namespace ArchitectureSample.Application.Blazor.Client.Dtos;

public record Create<T> where T : class
{
	public T Model { get; init; } = default!;
}