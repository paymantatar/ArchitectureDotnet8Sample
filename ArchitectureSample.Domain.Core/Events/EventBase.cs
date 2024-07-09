namespace ArchitectureSample.Domain.Core.Events;

public abstract class EventBase : IDomainEvent
{
	public string? EventType => GetType().FullName;

	public DateTime CreatedAt { get; } = DateTime.UtcNow;

	public string? CorrelationId { get; init; }

	public IDictionary<string, object> MetaData { get; } = new Dictionary<string, object>();

	public abstract void Flatten();
}