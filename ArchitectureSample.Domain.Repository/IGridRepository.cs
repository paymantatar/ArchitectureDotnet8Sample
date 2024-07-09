using ArchitectureSample.Domain.Core.Entities;
using ArchitectureSample.Domain.Specification;

namespace ArchitectureSample.Domain.Repository;

public interface IGridRepository<TEntity> where TEntity : EntityBase, IAggregateRoot
{
	ValueTask<long> CountAsync(IGridSpecification<TEntity> spec);
	
	Task<List<TEntity>> FindAsync(IGridSpecification<TEntity> spec);
}