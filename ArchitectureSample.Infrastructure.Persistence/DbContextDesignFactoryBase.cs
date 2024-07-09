using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ArchitectureSample.Infrastructure.Persistence;

public abstract class DbContextDesignFactoryBase<TDbContext> : IDesignTimeDbContextFactory<TDbContext>
	where TDbContext : DbContext
{
	public TDbContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<TDbContext>()
			.UseSqlServer(
				"",
				sqlOptions =>
				{
					sqlOptions.MigrationsAssembly(GetType().Assembly.FullName);
					sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null);
				}
			).UseSnakeCaseNamingConvention();

		return (TDbContext)Activator.CreateInstance(typeof(TDbContext), optionsBuilder.Options)!;
	}
}