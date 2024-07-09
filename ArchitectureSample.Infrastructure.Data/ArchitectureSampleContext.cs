using ArchitectureSample.Domain.Core.Entities;
using ArchitectureSample.Domain.Entities;
using ArchitectureSample.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArchitectureSample.Infrastructure.Data;

public class ArchitectureSampleContext : AppDbContextBase
{
	public ArchitectureSampleContext(DbContextOptions options) : base(options)
	{
	}

	public DbSet<Customer> Customers { get; set; } = default!;


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Customer>().ToTable("Customers");
		modelBuilder.Entity<Customer>().HasKey(x => x.Id);
		modelBuilder.Entity<Customer>().Property(x => x.Id).HasColumnType("uniqueidentifier");

		modelBuilder.Entity<Customer>().HasIndex(x => x.Created,"ix_Created").IsDescending();

		modelBuilder.Entity<Customer>().HasIndex(x => new { x.FirstName, x.LastName, x.DateOfBirth },"ix_Unique_FirstName-LastName-DateOfBirth").IsUnique();
		modelBuilder.Entity<Customer>().HasIndex(x => x.Email,"ix_Unique_Email").IsUnique();
		modelBuilder.Entity<Customer>().Ignore(x => x.DomainEvents);
	}
}

public class ArchitectureSampleContextDesignFactory : DbContextDesignFactoryBase<ArchitectureSampleContext>
{
}

public class Repository<TEntity> : RepositoryBase<ArchitectureSampleContext, TEntity> where TEntity : EntityBase, IAggregateRoot
{
	public Repository(ArchitectureSampleContext dbContext) : base(dbContext)
	{
	}
}