using ArchitectureSample.Application.Blazor.Client.Services;
using ArchitectureSample.Application.Blazor.Server.Components;
using ArchitectureSample.Application.Blazor.Server.Filters;
using ArchitectureSample.Infrastructure.Logging.Dtos;
using ArchitectureSample.Infrastructure.Logging.Helpers;
using Microsoft.AspNetCore.DataProtection;
using MudBlazor.Services;
using NLog.Config;
using NLog.Web;
using NLog;
using StackExchange.Redis;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.SetMinimumLevel(LogLevel.Trace).AddNLogWeb();

builder.Host.UseNLog();

LogManager.Configuration = new LoggingConfiguration()
	.AddErrorJsonFileConfiguration()
	.AddConsoleConfiguration()
	.AddMongoConfiguration(
		MongoOptions.CreateCustomOptions("Application", builder.Configuration.GetConnectionString("Mongo")!,
			"ArchitectureSample", "Blazor", NLog.LogLevel.Info, NLog.LogLevel.Fatal));

var logger = LogManager.Setup().GetLogger("Application");

var me = builder.WebHost.GetSetting("ASPNETCORE_URLS")!;

try
{
	builder.Services.AddRazorComponents()
		.AddInteractiveServerComponents()
		.AddInteractiveWebAssemblyComponents()
		.Services
		.AddNLog()
		.AddScoped(_ =>
		{
			var handler = new HttpClientHandler();
			handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
			return new HttpClient(handler) { BaseAddress = new Uri(builder.Configuration["ApiHost"]!) };
		})
		.AddScoped(_ =>
		{
			var handler = new HttpClientHandler();
			handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
			return new HttpClient(handler)
			{
				BaseAddress = new Uri(builder.WebHost.GetSetting("ASPNETCORE_URLS")!.Split(';')
					.Single(x => x.StartsWith("http://")))
			};
		})
		.AddControllers(options => options.Filters.Add<CustomExceptionFilter>())
		.Services
		.AddHealthChecks()
		.Services
		.AddSwaggerGen()
		.AddMudServices()
		.AddDataProtection()
			.PersistKeysToStackExchangeRedis(
				ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!)
				, "Blazor:Auth")
			.SetApplicationName("SharedCookieApp");

	builder.Services.AddScoped<ICustomerService, CustomerService>();

	var app = builder.Build();

	if (app.Environment.IsDevelopment())
		app.UseWebAssemblyDebugging();
	else
		app.UseExceptionHandler("/Error", createScopeForErrors: true).
			UseHsts();

	app
		//.UseHttpsRedirection()
		.UseStaticFiles()
		.UseAntiforgery()
		.UseSwagger()
		.UseSwaggerUI();

	app.MapControllers();

	app.MapRazorComponents<App>()
	    .AddInteractiveServerRenderMode()
	    .AddInteractiveWebAssemblyRenderMode()
	    .AddAdditionalAssemblies(typeof(ArchitectureSample.Application.Blazor.Client._Imports).Assembly);

	await app.RunAsync();
}
catch (Exception exception)
{
	logger.CustomError(new MongoLog
	{
		LoggerName = "Program",
		Message = "Program Stopped Because of Exception !",
		CustomData = new MongoLogCustomData
		{
			ClassName = nameof(Program),
			MethodName = "Main",
			Exception = exception
		}
	});
	throw;
}
finally
{
	NLog.LogManager.Shutdown();
}
