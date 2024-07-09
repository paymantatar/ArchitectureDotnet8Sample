using ArchitectureSample.Domain.Core.Events;
using ArchitectureSample.Domain.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ArchitectureSample.Infrastructure.Persistence;

public static class Extensions
{
	public static IServiceCollection AddSqlServerDbContext<TDbContext>(this IServiceCollection services,
		string connectionString, Action<DbContextOptionsBuilder>? doMoreDbContextOptionsConfigure = null,
		Action<IServiceCollection>? doMoreActions = null)
		where TDbContext : DbContext, IDbFacadeResolver, IDomainEventContext
	{
		services.AddDbContext<TDbContext>(options =>
		{
			options.UseSqlServer(connectionString, sqlOptions =>
			{
				sqlOptions.MigrationsAssembly(typeof(TDbContext).Assembly.GetName().Name);
				sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
			});

			doMoreDbContextOptionsConfigure?.Invoke(options);
		});

		services.AddScoped<IDbFacadeResolver>(provider => provider.GetService<TDbContext>()!);
		services.AddScoped<IDomainEventContext>(provider => provider.GetService<TDbContext>()!);

		services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TxBehavior<,>));

		services.AddHostedService<DbContextMigratorHostedService>();

		doMoreActions?.Invoke(services);

		return services;
	}

	public static IServiceCollection AddInMemoryDbContext<TDbContext>(this IServiceCollection services,
		Action<IServiceCollection>? doMoreActions = null)
		where TDbContext : DbContext, IDbFacadeResolver, IDomainEventContext
	{
		services.AddDbContext<TDbContext>(options => options.UseInMemoryDatabase("Database"));

		services.AddScoped<IDbFacadeResolver>(provider => provider.GetService<TDbContext>()!);
		services.AddScoped<IDomainEventContext>(provider => provider.GetService<TDbContext>()!);

		doMoreActions?.Invoke(services);

		return services;
	}

	public static IServiceCollection AddRepository(this IServiceCollection services, Type repoType)
	{
		services.Scan(scan => scan
			.FromAssembliesOf(repoType)
			.AddClasses(classes =>
				classes.AssignableTo(repoType)).As(typeof(IRepository<>)).WithScopedLifetime()
			.AddClasses(classes =>
				classes.AssignableTo(repoType)).As(typeof(IGridRepository<>)).WithScopedLifetime()
		);

		return services;
	}
}