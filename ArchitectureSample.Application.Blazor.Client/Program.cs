using ArchitectureSample.Application.Blazor.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddDataProtection()
	.PersistKeysToStackExchangeRedis(
		ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!)
		, "Blazor:Auth")
	.SetApplicationName("SharedCookieApp");

builder.Services.AddScoped<ICustomerService, CustomerService>()
	.AddScoped(_ =>
	{
		var handler = new HttpClientHandler();
		//handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
		return new HttpClient(handler)
		{
			BaseAddress = new Uri("localhost:9090")
		};
	});

await builder.Build().RunAsync();