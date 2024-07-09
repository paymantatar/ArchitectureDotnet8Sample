﻿using ArchitectureSample.Domain.Core.Entities;
using ArchitectureSample.Domain.Repository;
using ArchitectureSample.Domain.Specification;
using Microsoft.EntityFrameworkCore;

namespace ArchitectureSample.Infrastructure.Persistence;

public class RepositoryBase<TDbContext, TEntity> : IRepository<TEntity>, IGridRepository<TEntity>
	where TEntity : EntityBase, IAggregateRoot
	where TDbContext : DbContext
{
	private readonly TDbContext _dbContext;

	protected RepositoryBase(TDbContext dbContext) =>
		_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

	public TEntity? FindById(Guid id) =>
		_dbContext.Set<TEntity>().SingleOrDefault(e => e.Id == id);

	public async Task<TEntity?> FindOneAsync(ISpecification<TEntity?> spec) =>
		await GetQuery(_dbContext.Set<TEntity>(), spec).FirstOrDefaultAsync();

	public async Task<List<TEntity?>> FindAsync(ISpecification<TEntity?> spec) =>
		await GetQuery(_dbContext.Set<TEntity>(), spec).ToListAsync();

	public async ValueTask<long> CountAsync(IGridSpecification<TEntity> spec)
	{
		spec.IsPagingEnabled = false;
		return await ValueTask.FromResult(GetQuery(_dbContext.Set<TEntity>(), spec).LongCount());
	}

	public async Task<List<TEntity>> FindAsync(IGridSpecification<TEntity> spec) =>
		await GetQuery(_dbContext.Set<TEntity>(), spec).ToListAsync();

	public async Task<TEntity> AddAsync(TEntity entity)
	{
		await _dbContext.Set<TEntity>().AddAsync(entity);

		await _dbContext.SaveChangesAsync();

		return entity;
	}

	public async Task<TEntity> UpdateAsync(TEntity entity)
	{
		_dbContext.Set<TEntity>().Update(entity);

		await _dbContext.SaveChangesAsync();

		return entity;
	}

	public async Task RemoveAsync(TEntity entity)
	{
		_dbContext.Set<TEntity>().Remove(entity);

		await _dbContext.SaveChangesAsync();
	}

	public async Task<TEntity?> RemoveAsync(Guid id)
	{
		var exist = FindById(id);

		if (exist is null) return null;

		_dbContext.Set<TEntity>().Remove(exist);

		await _dbContext.SaveChangesAsync();

		return exist;
	}


	private static IQueryable<TEntity?> GetQuery(IQueryable<TEntity?> inputQuery,
		ISpecification<TEntity?> specification)
	{
		var query = inputQuery;

		if (specification.Criteria is not null)
			query = query.Where(specification.Criteria);

		query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

		query = specification.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

		if (specification.OrderBy is not null)
			query = query.OrderBy(specification.OrderBy);
		else if (specification.OrderByDescending is not null)
			query = query.OrderByDescending(specification.OrderByDescending);

		if (specification.GroupBy is not null)
			query = query.GroupBy(specification.GroupBy).SelectMany(x => x);

		if (specification.IsPagingEnabled)
			query = query.Skip((specification.Skip - 1) * specification.Take)
				.Take(specification.Take);

		return query;
	}

	private static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,
		IGridSpecification<TEntity> specification)
	{
		var query = inputQuery;

		if (specification.Criterias is not null && specification.Criterias.Count > 0)
		{
			var expr = specification.Criterias.First();
			for (var i = 1; i < specification.Criterias.Count; i++)
				expr = expr.And(specification.Criterias[i]);

			query = query.Where(expr);
		}

		query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

		query = specification.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

		if (specification.OrderBy is not null)
			query = query.OrderBy(specification.OrderBy);
		else if (specification.OrderByDescending is not null)
			query = query.OrderByDescending(specification.OrderByDescending);

		if (specification.GroupBy is not null)
			query = query.GroupBy(specification.GroupBy).SelectMany(x => x);

		if (specification.IsPagingEnabled)
			query = query.Skip((specification.Skip - 1) * specification.Take)
				.Take(specification.Take);

		return query;
	}
}