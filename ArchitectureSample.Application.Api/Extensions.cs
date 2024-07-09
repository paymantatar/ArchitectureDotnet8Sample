using ArchitectureSample.Application.Api.Filters;
using ArchitectureSample.Application.Commands;
using ArchitectureSample.Application.Queries;
using ArchitectureSample.Infrastructure.Cache;
using ArchitectureSample.Infrastructure.Core.Validators;
using ArchitectureSample.Infrastructure.Data;
using ArchitectureSample.Infrastructure.Logging.Helpers;
using ArchitectureSample.Infrastructure.Persistence;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Prometheus;

namespace ArchitectureSample.Application.Api;

public static class Extensions
{
	public static IServiceCollection AddCoreServices(this IServiceCollection services,
	    IConfiguration config, Type apiType, IWebHostEnvironment environment)
	{
		services.AddHealthChecks()
			.Services
			.AddNLog()
			.AddSwagger(apiType)
			.AddCustomMediatR(new[] { typeof(GetCustomer), typeof(CreateCustomer) })
			.AddCustomValidators(new[] { typeof(GetCustomer), typeof(CreateCustomer) })
			.AddCors(options => options.AddPolicy("BlazorOrigin",
				builder => builder
					.WithOrigins(config["BlazorHost"]!)
					.AllowAnyHeader()
					.AllowAnyMethod()))
			.AddControllers(options => options.Filters.Add<CustomExceptionFilter>());

		if (environment.IsProduction())
			services.AddSqlServerDbContext<ArchitectureSampleContext>(
					config.GetConnectionString("SqlServer") ?? "",
					null,
					svc => svc.AddRepository(typeof(Repository<>))
				)
				.AddRedisCache(config.GetConnectionString("Redis")!, typeof(Program).Assembly.GetName().Name?.Split('.').First() ?? "Cache");
		else
			services.AddInMemoryDbContext<ArchitectureSampleContext>(
				svc => svc.AddRepository(typeof(Repository<>))
			).AddInMemoryCache();

		return services;
	}

	public static IApplicationBuilder UseCoreApplication(this WebApplication app, IWebHostEnvironment env)
	{
		if (env.IsDevelopment())
			app.UseDeveloperExceptionPage();

		app.UseRouting()
			.UseHttpMetrics()
			.UseCors("BlazorOrigin")
			.UseSwaggerCore();
		
		app.MapControllers();
		app.MapHealthChecks("HealthChecks");
		app.MapMetrics();

		return app;
	}

	public static IServiceCollection AddCustomMediatR(this IServiceCollection services, Type[]? types = null,
		Action<IServiceCollection>? doMoreActions = null)
	{
		services.AddHttpContextAccessor()
			.AddMediatR(x =>
			{
				foreach (var type in types!)
					x.RegisterServicesFromAssemblyContaining(type);
			})
			.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>))
			.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

		doMoreActions?.Invoke(services);

		return services;
	}

	public static IServiceCollection AddCustomValidators(this IServiceCollection services, Type[] types) =>
		services
			.AddFluentValidationAutoValidation()
			.Scan(scan => scan
				.FromAssembliesOf(types)
				.AddClasses(c => c.AssignableTo(typeof(IValidator<>)))
				.AsImplementedInterfaces()
				.WithTransientLifetime());
}

