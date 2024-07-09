namespace ArchitectureSample.Domain.Core.Events;

public interface IDomainEventContext
{
	IEnumerable<EventBase> GetDomainEvents();
}