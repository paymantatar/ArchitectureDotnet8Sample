using ArchitectureSample.Domain.Core.Events;

namespace ArchitectureSample.Domain.Core.Entities;

public abstract class EntityRootBase : EntityBase, IAggregateRoot
{
	public HashSet<EventBase>? DomainEvents { get; private set; }

	public void AddDomainEvent(EventBase eventItem)
	{
		DomainEvents ??= new();
		DomainEvents.Add(eventItem);
	}

	public void RemoveDomainEvent(EventBase eventItem) =>
		DomainEvents?.Remove(eventItem);
}