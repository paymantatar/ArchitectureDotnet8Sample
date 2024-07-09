using Microsoft.Extensions.DependencyInjection;

namespace ArchitectureSample.Infrastructure.Cache;

public static class Extensions
{
	public static IServiceCollection AddRedisCache(this IServiceCollection services, string connectionString, string prefix) =>
		services.AddStackExchangeRedisCache(options =>
		{
			options.Configuration = connectionString;
			options.InstanceName = $"{prefix}:";
		});

	public static IServiceCollection AddInMemoryCache(this IServiceCollection services) =>
		services.AddDistributedMemoryCache(options =>
		{
			options.ExpirationScanFrequency = TimeSpan.FromMinutes(1);
		});
}