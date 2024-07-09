namespace ArchitectureSample.Domain.Core.Events;

public class DaprPubSubNameAttribute : Attribute
{
	public string PubSubName { get; set; } = "PubSub";
}