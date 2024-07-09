using ArchitectureSample.Domain.Core.Cqrs;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ArchitectureSample.Infrastructure.Core.Validators;

public static class Extensions
{
	public static async Task HandleValidation<TRequest>(this IValidator<TRequest> validator, TRequest request)
	{
		var validationResult = await validator.ValidateAsync(request);
		if (!validationResult.IsValid)
			throw new ValidationException(validationResult.Errors.ToString());
	}

	

	public static IServiceCollection AddSwagger(this IServiceCollection services, Type anchor) => 
		services.AddSwaggerGen();

	public static IApplicationBuilder UseSwaggerCore(this IApplicationBuilder app) =>
		app.UseSwagger().UseSwaggerUI();

	public static TResult SafeGetListQuery<TResult, TResponse>(this HttpContext httpContext, string query)
		where TResult : IListQuery<TResponse>, new() where TResponse : notnull
	{
		var queryModel = new TResult();
		if (!(string.IsNullOrEmpty(query) || query == "{}"))
		{
			queryModel = JsonConvert.DeserializeObject<TResult>(query);
		}

		httpContext.Response.Headers.Append("x-query",
			JsonConvert.SerializeObject(queryModel,
				new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));

		return queryModel!;
	}
}