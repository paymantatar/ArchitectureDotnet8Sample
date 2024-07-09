using ArchitectureSample.Application.Commands;
using ArchitectureSample.Application.Dtos;
using ArchitectureSample.Application.Queries;
using ArchitectureSample.Domain.Core.Cqrs;
using ArchitectureSample.Infrastructure.Core.Validators;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArchitectureSample.Application.Api.Controllers;

public class BaseController : Controller
{
	private ISender? _mediator;

	protected ISender? Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();
}

[ApiController]
[Route("[controller]")]
public class CustomerController : BaseController
{
	[HttpGet("/api/v1/customers")]
	public async Task<IActionResult> HandleGetCustomersAsync([FromHeader(Name = "x-query")] string query, CancellationToken cancellationToken = new()) =>
		Ok(await Mediator!.Send(HttpContext.SafeGetListQuery<GetCustomer.Query, ListResultModel<CustomerDto>>(query), cancellationToken));

	[HttpPost("/api/v1/customers")]
	public async Task<IActionResult> HandleCreateCustomerAsync([FromBody] CreateCustomer.Command request, CancellationToken cancellationToken = new()) =>
		Ok(await Mediator!.Send(request, cancellationToken));

	[HttpPut("/api/v1/customers")]
	public async Task<IActionResult> HandleUpdateCustomerAsync([FromBody] UpdateCustomer.UpdateCommand request, CancellationToken cancellationToken = new()) =>
		Ok(await Mediator!.Send(request, cancellationToken));

	[HttpDelete("/api/v1/customers")]
	public async Task<IActionResult> HandleDeleteCustomerAsync([FromBody] DeleteCustomer.DeleteCommand request, CancellationToken cancellationToken = new()) =>
		Ok(await Mediator!.Send(request, cancellationToken));
}