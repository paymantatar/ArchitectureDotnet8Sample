using System.Text.Json;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ArchitectureSample.Infrastructure.Core.Validators;

public class RequestValidationBehavior<TRequest, TResponse>(IValidator<TRequest> validator,
		ILogger<RequestValidationBehavior<TRequest, TResponse>> logger)
	: IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
	where TResponse : notnull
{
	private readonly IValidator<TRequest> _validator = validator ?? throw new ArgumentNullException(nameof(validator));
	private readonly ILogger<RequestValidationBehavior<TRequest, TResponse>> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

	public async Task<TResponse> Handle(TRequest request,
		RequestHandlerDelegate<TResponse> next,
		CancellationToken cancellationToken)
	{
		_logger.LogInformation(
			"[{Prefix}] Handle request={X-RequestData} and response={X-ResponseData}",
			nameof(RequestValidationBehavior<TRequest, TResponse>), typeof(TRequest).Name, typeof(TResponse).Name);

		_logger.LogDebug($"Handling {typeof(TRequest).FullName} with content {JsonSerializer.Serialize(request)}");

		await _validator.HandleValidation(request);

		var response = await next();

		_logger.LogInformation($"Handled {typeof(TRequest).FullName}");
		return response;
	}
}