namespace ArchitectureSample.Domain.Core.Cqrs;

public interface IListQuery<TResponse> : IQuery<TResponse>
	where TResponse : notnull
{
	public List<string> Includes { get; init; }
	
	public List<FilterModel> Filters { get; init; }
	
	public List<string> Sorts { get; init; }
	
	public int Page { get; init; }
	
	public int PageSize { get; init; }
}