using ArchitectureSample.Domain.Core.Entities;

namespace ArchitectureSample.Domain.Core.Cqrs;

public interface IUpdateCommand<TRequest, TResponse> : ICommand<TResponse>, ITxRequest
	where TRequest : notnull
	where TResponse : notnull
{
	public TRequest Model { get; init; }
}