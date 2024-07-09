namespace ArchitectureSample.Infrastructure.Logging.Dtos;

public record struct MongoLog()
{
    public string? Message { get; set; } = null;

    public string? LoggerName { get; set; } = "Info";

    public MongoLogCustomData CustomData { get; set; } = new();
}