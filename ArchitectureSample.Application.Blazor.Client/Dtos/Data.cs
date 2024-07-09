namespace ArchitectureSample.Application.Blazor.Client.Dtos;

public record Data<T> where T : class
{
	public List<T>? Items { get; set; }

	public int TotalItems { get; set; }

	public int Page { get; set; }

	public int PageSize { get; set; }
}