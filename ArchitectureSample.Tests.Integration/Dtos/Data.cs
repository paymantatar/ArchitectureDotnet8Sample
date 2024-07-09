namespace ArchitectureSample.Tests.Integration.Dtos;

public class Data<T> where T : class
{
	public List<T>? Items { get; set; }

	public int TotalItems { get; set; }

	public int Page { get; set; }

	public int PageSize { get; set; }
}