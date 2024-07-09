namespace ArchitectureSample.Tests.Integration.Helpers;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class TestOrderAttribute(int priority) : Attribute
{
    public int Priority { get; set; } = priority;
}