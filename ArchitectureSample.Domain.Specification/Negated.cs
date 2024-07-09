using System.Linq.Expressions;

namespace ArchitectureSample.Domain.Specification;

public class Negated<T>(ISpecification<T> inner) : SpecificationBase<T>
{
	public override Expression<Func<T, bool>> Criteria
	{
		get
		{
			var objParam = Expression.Parameter(typeof(T), "obj");

			var expression = Expression.Lambda<Func<T, bool>>(
				Expression.Not(
					Expression.Invoke(inner.Criteria!, objParam)
				),
				objParam
			);

			return expression;
		}
	}
}