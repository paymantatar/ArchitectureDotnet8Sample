namespace ArchitectureSample.Application.Blazor.Client.Dtos;

public class QueryApiRequest
{
	public List<string> Includes { get; init; } = new();

	public List<FilterModel> Filters { get; set; } = new();

	public List<string> Sorts { get; set; } = new();

	public int Page { get; init; } = 1;

	public int PageSize { get; init; } = 20;
}