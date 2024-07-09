using ArchitectureSample.Domain.Core.Cqrs;
using ArchitectureSample.Infrastructure.Logging.Dtos;
using ArchitectureSample.Infrastructure.Logging.Helpers;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ILogger = NLog.ILogger;

namespace ArchitectureSample.Application.Blazor.Server.Filters;

public class CustomExceptionFilter : ExceptionFilterAttribute
{
	private readonly ILogger _logger;

	public CustomExceptionFilter(IEnumerable<ILogger>? loggers) =>
		_logger = loggers?.SingleOrDefault(x => x.Name == "Application") ?? throw new ArgumentNullException("ILogger", "ILogger With Name 'Application' Not Registered !");

	public override void OnException(ExceptionContext filterContext)
	{
		if (filterContext.ExceptionHandled)
			return;
		var changedId = Guid.NewGuid();
		var mongoLog = new MongoLog
		{
			LoggerName = "Error",
			Message = "Error in " + filterContext.ActionDescriptor.RouteValues["controller"] + "-" + filterContext.ActionDescriptor.RouteValues["action"],
			CustomData = new MongoLogCustomData
			{
				MethodName = filterContext.ActionDescriptor.RouteValues["action"],
				ClassName = filterContext.ActionDescriptor.RouteValues["controller"],
				ClientIp = filterContext.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString(),
				ServerIp = filterContext.HttpContext.Features.Get<IHttpConnectionFeature>()?.LocalIpAddress?.MapToIPv4().ToString(),
				Exception = filterContext.Exception,
				ReferencesCode = changedId
			}
		};
		_logger.CustomError(mongoLog);

		filterContext.Result = new JsonResult(new ResultModel<Guid>(changedId, true, "Internal server error"));
		filterContext.ExceptionHandled = true;
	}

	public override Task OnExceptionAsync(ExceptionContext filterContext)
	{
		if (filterContext.ExceptionHandled)
			return Task.CompletedTask;

		var changedId = Guid.NewGuid();
		var mongoLog = new MongoLog
		{
			LoggerName = "Error",
			Message = "Error in " + filterContext.ActionDescriptor.RouteValues["controller"] + "-" + filterContext.ActionDescriptor.RouteValues["action"],
			CustomData = new MongoLogCustomData
			{
				MethodName = filterContext.ActionDescriptor.RouteValues["action"],
				ClassName = filterContext.ActionDescriptor.RouteValues["controller"],
				ClientIp = filterContext.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString(),
				ServerIp = filterContext.HttpContext.Features.Get<IHttpConnectionFeature>()?.LocalIpAddress?.MapToIPv4().ToString(),
				Exception = filterContext.Exception,
				ReferencesCode = changedId
			}
		};
		_logger.CustomError(mongoLog);

		filterContext.Result = new JsonResult(new ResultModel<Guid>(changedId, true, "Internal server error"));
		filterContext.ExceptionHandled = true;
		return Task.CompletedTask;
	}
}