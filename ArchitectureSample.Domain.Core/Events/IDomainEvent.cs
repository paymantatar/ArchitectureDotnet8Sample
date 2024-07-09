using MediatR;

namespace ArchitectureSample.Domain.Core.Events;

public interface IDomainEvent : INotification
{
	DateTime CreatedAt { get; }
	IDictionary<string, object> MetaData { get; }
}