using ArchitectureSample.Domain.Core.Cqrs;

namespace ArchitectureSample.Infrastructure.Logging.Dtos;

public record struct MongoLogCustomData(
    object? Request,
    ResultModel<object> Response,
    TimeSpan ElapsedTime,
    string? ClientIp,
    string? ServerIp,
    string? ClassName,
    string? MethodName,
    string? Username,
    Exception? Exception,
    Guid ReferencesCode);