namespace ArchitectureSample.Tests.Integration.Dtos;

public class QueryApiResponse<T> where T : class
{
	public Data<T>? Data { get; set; }

	public bool IsError { get; set; }

	public object? ErrorMessage { get; set; }
}