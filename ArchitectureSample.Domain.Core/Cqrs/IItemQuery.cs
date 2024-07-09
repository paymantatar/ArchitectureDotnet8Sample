namespace ArchitectureSample.Domain.Core.Cqrs;

public interface IItemQuery<TId, TResponse> : IQuery<TResponse>
	where TId : struct
	where TResponse : notnull
{
	public List<string> Includes { get; init; }
	public TId Id { get; init; }
}