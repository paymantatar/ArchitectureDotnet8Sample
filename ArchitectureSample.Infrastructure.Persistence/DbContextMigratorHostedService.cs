using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace ArchitectureSample.Infrastructure.Persistence;

internal class DbContextMigratorHostedService(IServiceProvider serviceProvider,
		ILogger<DbContextMigratorHostedService> logger)
	: IHostedService
{
	public async Task StartAsync(CancellationToken cancellationToken) =>
		await CreatePolicy(3, logger, nameof(DbContextMigratorHostedService)).ExecuteAsync(async () =>
		{
			using var scope = serviceProvider.CreateScope();
			var dbFacadeResolver = scope.ServiceProvider.GetRequiredService<IDbFacadeResolver>();

			if (!await dbFacadeResolver.Database.CanConnectAsync(cancellationToken))
			{
				throw new Exception("Couldn't connect database.");
			}

			await dbFacadeResolver.Database.MigrateAsync(cancellationToken);
			logger.LogInformation("Done migration database schema.");
		});

	public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

	private static AsyncRetryPolicy CreatePolicy(int retries, ILogger logger, string prefix) =>
		Policy.Handle<Exception>().WaitAndRetryAsync(
			retries,
			_ => TimeSpan.FromSeconds(5),
			(exception, _, retry, _) =>
			{
				logger.LogWarning(exception,
					"[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}",
					prefix, exception.GetType().Name, exception.Message, retry, retries);
			}
		);
}