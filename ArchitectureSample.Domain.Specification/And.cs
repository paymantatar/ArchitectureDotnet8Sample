using System.Linq.Expressions;

namespace ArchitectureSample.Domain.Specification;

public class And<T>(ISpecification<T> left,
	ISpecification<T> right) : SpecificationBase<T>
{
	public override Expression<Func<T, bool>> Criteria
	{
		get
		{
			var objParam = Expression.Parameter(typeof(T), "obj");

			var expression = Expression.Lambda<Func<T, bool>>(
				Expression.AndAlso(
					Expression.Invoke(left.Criteria!, objParam),
					Expression.Invoke(right.Criteria!, objParam)
				),
				objParam
			);

			return expression;
		}
	}
}