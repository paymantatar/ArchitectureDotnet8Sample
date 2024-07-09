using NLog;

namespace ArchitectureSample.Infrastructure.Logging.Dtos;

public record struct MongoOptions
{
    private MongoOptions(
        string name,
        string connectionString,
        string databaseName,
        string collectionName,
        LogLevel minLevel,
        LogLevel maxLevel)
    {
        ConnectionString = connectionString;
        DatabaseName = databaseName;
        CollectionName = collectionName;
        Name = name;
        MinLevel = minLevel;
        MaxLevel = maxLevel;
    }

    public static MongoOptions CreateApplicationOptions(
        string connectionString,
        string databaseName) =>
        new("Application", connectionString, databaseName, "AppLog", LogLevel.Info, LogLevel.Fatal);

    public static MongoOptions CreateCustomOptions(
        string name,
        string connectionString,
        string databaseName,
        string collectionName,
        LogLevel minLevel,
        LogLevel maxLevel) =>
        new(name, connectionString, databaseName, collectionName, minLevel, maxLevel);

    public string ConnectionString { get; set; }

    public string DatabaseName { get; set; }

    public string CollectionName { get; set; }

    public string Name { get; set; }

    public LogLevel MinLevel { get; set; }

    public LogLevel MaxLevel { get; set; }
}