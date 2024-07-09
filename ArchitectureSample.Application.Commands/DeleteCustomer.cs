using ArchitectureSample.Application.Dtos;
using ArchitectureSample.Domain.Core.Cqrs;
using ArchitectureSample.Domain.Entities;
using ArchitectureSample.Domain.Repository;
using ArchitectureSample.Infrastructure.Core.Validators;
using FluentValidation;
using MediatR;

namespace ArchitectureSample.Application.Commands;

public class DeleteCustomer
{
	public record DeleteCommand : ICreateCommand<DeleteCommand.DeleteCustomerModel, CustomerDto>
	{
		public DeleteCustomerModel Model { get; init; } = default!;

		public record DeleteCustomerModel(Guid Id);

		internal class Validator : AbstractValidator<DeleteCommand>
		{
			public Validator()
			{
				RuleFor(x => x.Model.Id).NotEmpty().GuidValidator();
			}
		}

		internal class Handler(IRepository<Customer> repository) : IRequestHandler<DeleteCommand, ResultModel<CustomerDto>>
		{
			public async Task<ResultModel<CustomerDto>> Handle(DeleteCommand request, CancellationToken cancellationToken)
			{
				var deleted = await repository.RemoveAsync(
					request.Model.Id);

				if (deleted is null)
					return new ResultModel<CustomerDto>(new CustomerDto());

				return ResultModel<CustomerDto>.Create(new CustomerDto
				{
					Id = deleted.Id,
					FirstName = deleted.FirstName,
					LastName = deleted.LastName,
					DateOfBirth = deleted.DateOfBirth,
					PhoneNumber = deleted.PhoneNumber,
					Created = deleted.Created,
					Updated = deleted.Updated,
					Email = deleted.Email,
					BankAccount = deleted.BankAccount
				});
			}
		}
	}
}