using MediatR;

namespace ArchitectureSample.Domain.Core.Cqrs;

public interface ICommand<T> : IRequest<ResultModel<T>>
	where T : notnull
{
}