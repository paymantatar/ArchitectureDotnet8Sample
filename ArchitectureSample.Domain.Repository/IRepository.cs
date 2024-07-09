using ArchitectureSample.Domain.Core.Entities;
using ArchitectureSample.Domain.Specification;

namespace ArchitectureSample.Domain.Repository;

public interface IRepository<TEntity> where TEntity : EntityBase, IAggregateRoot
{
	TEntity? FindById(Guid id);

	Task<TEntity?> FindOneAsync(ISpecification<TEntity?> spec);

	Task<List<TEntity?>> FindAsync(ISpecification<TEntity?> spec);

	Task<TEntity> AddAsync(TEntity entity);

	Task RemoveAsync(TEntity entity);

	Task<TEntity> UpdateAsync(TEntity entity);

	Task<TEntity?> RemoveAsync(Guid id);
}