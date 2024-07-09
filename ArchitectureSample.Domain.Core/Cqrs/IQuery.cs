using MediatR;

namespace ArchitectureSample.Domain.Core.Cqrs;

public interface IQuery<T> : IRequest<ResultModel<T>>
	where T : notnull
{
}