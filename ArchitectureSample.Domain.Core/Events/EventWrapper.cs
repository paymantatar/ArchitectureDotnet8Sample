using MediatR;

namespace ArchitectureSample.Domain.Core.Events;

public class EventWrapper(IDomainEvent @event) : INotification
{
	public IDomainEvent Event { get; } = @event;
}