using ArchitectureSample.Domain.Core.Cqrs;
using ArchitectureSample.Domain.Entities;
using ArchitectureSample.Domain.Specification;

namespace ArchitectureSample.Infrastructure.Core.Specs;

public sealed class CustomerListQuerySpec<TResponse> : GridSpecificationBase<Customer> where TResponse : notnull
{
	public CustomerListQuerySpec(IListQuery<ListResultModel<TResponse>> gridQueryInput)
	{
		ApplyIncludeList(gridQueryInput.Includes);

		ApplyFilterList(gridQueryInput.Filters);

		ApplySortingList(gridQueryInput.Sorts);

		ApplyPaging(gridQueryInput.Page, gridQueryInput.PageSize);
	}
}