using System.Text.Json;
using ArchitectureSample.Application.Dtos;
using ArchitectureSample.Domain.Core.Cqrs;
using ArchitectureSample.Domain.Entities;
using ArchitectureSample.Domain.Repository;
using ArchitectureSample.Infrastructure.Core.Specs;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace ArchitectureSample.Application.Queries;

public class GetCustomer
{
	public class Query : IListQuery<ListResultModel<CustomerDto>>
	{
		public List<string> Includes { get; init; } = new();

		public List<FilterModel> Filters { get; init; } = new();

		public List<string> Sorts { get; init; } = new();

		public int Page { get; init; } = 1;

		public int PageSize { get; init; } = 20;

		internal class Validator : AbstractValidator<Query>
		{
			public Validator()
			{
				RuleFor(x => x.Page)
				    .GreaterThanOrEqualTo(1).WithMessage("Page should at least greater than or equal to 1.");

				RuleFor(x => x.PageSize)
				    .GreaterThanOrEqualTo(1).WithMessage("PageSize should at least greater than or equal to 1.");
			}
		}

		internal class Handler(IGridRepository<Customer> customerRepository /*, IDistributedCache distributedCache*/) : IRequestHandler<Query, ResultModel<ListResultModel<CustomerDto>>>
		{
			private readonly IGridRepository<Customer> _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));

			public async Task<ResultModel<ListResultModel<CustomerDto>>> Handle(Query request,
			    CancellationToken cancellationToken)
			{
				if (request == null) throw new ArgumentNullException(nameof(request));

				var spec = new CustomerListQuerySpec<CustomerDto>(request);

				//var cacheKey = "Customers:" + JsonSerializer.Serialize(request);

				//var cache = await distributedCache.GetStringAsync(cacheKey, cancellationToken);

				//if (cache != null)
				//{
				//	var cacheData = JsonSerializer.Deserialize<CacheContract>(cache);

				//	var loadedData = JsonSerializer.Deserialize<List<CustomerDto>>(cacheData.Data);

				//	return ResultModel<ListResultModel<CustomerDto>>.Create(ListResultModel<CustomerDto>.Create(loadedData, cacheData.TotalCount,
				//		request.Page, request.PageSize));
				//}

				var customers = await _customerRepository.FindAsync(spec);

				var customerModels = customers.Select(x => new CustomerDto
				{
					Id = x.Id,
					FirstName = x.FirstName,
					LastName = x.LastName,
					DateOfBirth = x.DateOfBirth,
					Email = x.Email,
					Created = x.Created,
					Updated = x.Updated,
					PhoneNumber = x.PhoneNumber,
					BankAccount = x.BankAccount
				});

				var totalCustomers = await _customerRepository.CountAsync(spec);

				//await distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(new CacheContract
				//{
				//	Data = JsonSerializer.Serialize(customers),
				//	TotalCount = totalCustomers
				//}),
				//	cancellationToken);

				var resultModel = ListResultModel<CustomerDto>.Create(
				    customerModels.ToList(), totalCustomers, request.Page, request.PageSize);

				return ResultModel<ListResultModel<CustomerDto>>.Create(resultModel);
			}
		}
	}
}

public class CacheContract
{
	public string? Data { get; set; }

	public long TotalCount { get; set; }
}