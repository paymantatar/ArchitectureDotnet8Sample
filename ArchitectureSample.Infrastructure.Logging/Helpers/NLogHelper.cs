using ArchitectureSample.Infrastructure.Logging.Dtos;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Mongo;
using NLog.Targets;

namespace ArchitectureSample.Infrastructure.Logging.Helpers;

public static class NLogHelper
{
	public static LoggingConfiguration AddMongoConfiguration(
	    this LoggingConfiguration config,
	    MongoOptions options)
	{
		var mongoTarget = new MongoTarget
		{
			ConnectionString = options.ConnectionString,
			Name = options.Name,
			IncludeDefaults = false,
			DatabaseName = options.DatabaseName,
			CollectionName = options.CollectionName,
			Fields =
			{
				new MongoField
				{
					Name = "Date",
					BsonType = "DateTime",
					Layout = Layout.FromString("${date}")
				},
				new MongoField
				{
					Name = "Level",
					Layout = Layout.FromString("${level}")
				},
				new MongoField
				{
					Name = "Message",
					Layout = Layout.FromString("${message}")
				},
				new MongoField
				{
					Name = "Logger",
					Layout = Layout.FromString("${logger}")
				},
				new MongoField
				{
					Name = "Detail",
					BsonType = "Object",
					Layout = new JsonLayout
					{
						MaxRecursionLimit = 10,
						IncludeEventProperties = true
					}
				}
			}
		};
		config.AddRule(options.MinLevel, options.MaxLevel, mongoTarget, options.Name);
		return config;
	}

	public static LoggingConfiguration AddErrorJsonFileConfiguration(
	    this LoggingConfiguration config)
	{
		var fileTarget = new FileTarget
		{
			FileName = Layout.FromString("${basedir}/logs/AppLog-${level}-${shortdate}.json"),
			Layout = new JsonLayout
			{
				Attributes =
		{
		    new JsonAttribute("time", Layout.FromString("${date:format=O}")),
		    new JsonAttribute("message", Layout.FromString("${message}")),
		    new JsonAttribute("logger", Layout.FromString("${logger}")),
		    new JsonAttribute("level", Layout.FromString("${level}")),
		    new JsonAttribute("Detail", new JsonLayout
		    {
			MaxRecursionLimit = 10,
			IncludeEventProperties = true
		    })
		}
			}
		};
		config.AddRule(LogLevel.Error, LogLevel.Fatal, fileTarget);
		return config;
	}

	public static LoggingConfiguration AddConsoleConfiguration(this LoggingConfiguration config)
	{
		var consoleTarget = new ConsoleTarget
		{
			Name = "Console",
			Layout = new JsonLayout
			{
				MaxRecursionLimit = 10,
				IncludeEventProperties = true
			},
			StdErr = true
		};
		config.AddRule(LogLevel.Error, LogLevel.Fatal, consoleTarget);
		return config;
	}

	public static void CustomInfo(this ILogger logger, MongoLog mongoLog) =>
	    logger.Log(mongoLog, LogLevel.Info);

	public static void CustomError(this ILogger logger, MongoLog mongoLog) =>
	    logger.Log(mongoLog, LogLevel.Error);

	public static void CustomFatal(this ILogger logger, MongoLog mongoLog) =>
	    logger.Log(mongoLog, LogLevel.Fatal);

	public static void CustomWarn(this ILogger logger, MongoLog mongoLog) =>
	    logger.Log(mongoLog, LogLevel.Warn);

	public static void CustomTrace(this ILogger logger, MongoLog mongoLog) =>
	    logger.Log(mongoLog, LogLevel.Trace);

	private static void Log(this ILogger logger, MongoLog mongoLog, LogLevel logLevel)
	{
		var logEventInfo = new LogEventInfo(logLevel, mongoLog.LoggerName, mongoLog.Message);
		foreach (var property in typeof(MongoLogCustomData).GetProperties())
			logEventInfo.Properties.Add(property.Name, property.GetValue(mongoLog.CustomData));
		logger.Log(logLevel, logEventInfo);
	}
}