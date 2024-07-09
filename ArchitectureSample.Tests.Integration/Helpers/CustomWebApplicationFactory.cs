using Microsoft.AspNetCore.Mvc.Testing;

namespace ArchitectureSample.Tests.Integration.Helpers;

public class CustomWebApplicationFactory<T> : WebApplicationFactory<T> where T : class
{
	protected override void ConfigureWebHost(IWebHostBuilder builder) => 
		builder.UseEnvironment("Testing");
}