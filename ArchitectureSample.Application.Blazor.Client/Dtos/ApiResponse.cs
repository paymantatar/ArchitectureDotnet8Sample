namespace ArchitectureSample.Application.Blazor.Client.Dtos;

public record ApiResponse<T> where T : class
{
	public T? Data { get; set; }

	public bool IsError { get; set; }

	public object? ErrorMessage { get; set; }
}
