using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace ArchitectureSample.Infrastructure.Logging.Helpers;

public static class Extensions
{
    public static IServiceCollection AddNLog(this IServiceCollection services) =>
        services.AddSingleton<ILogger>(_ => LogManager.Setup().GetLogger("Application"));
}