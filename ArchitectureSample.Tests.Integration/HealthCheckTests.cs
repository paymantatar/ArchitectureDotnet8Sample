using System.Net;
using ArchitectureSample.Tests.Integration.Helpers;
using FluentAssertions;

namespace ArchitectureSample.Tests.Integration;

public class HealthCheckTests(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>
{
	[Fact]
	public async Task HealthCheck_ReturnOk()
	{
		var response = await factory.CreateDefaultClient().GetAsync("/HealthChecks");

		response.StatusCode.Should().Be(HttpStatusCode.OK);
		response.Content.Headers.ContentLength.Should().BeGreaterThan(0);
		var result = await response.Content.ReadAsStringAsync();
		result.Should().NotBeNullOrWhiteSpace();
		result.Should().Be("Healthy");
	}
}