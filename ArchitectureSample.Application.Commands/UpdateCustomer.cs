using ArchitectureSample.Application.Dtos;
using ArchitectureSample.Domain.Core.Cqrs;
using ArchitectureSample.Domain.Entities;
using ArchitectureSample.Domain.Repository;
using ArchitectureSample.Infrastructure.Core.Validators;
using FluentValidation;
using MediatR;

namespace ArchitectureSample.Application.Commands;

public class UpdateCustomer
{
	public record UpdateCommand : ICreateCommand<UpdateCommand.UpdateCustomerModel, CustomerDto>
	{
		public UpdateCustomerModel Model { get; init; } = default!;

		public record UpdateCustomerModel(Guid Id, string FirstName, string LastName, DateTime BirthOfDate, string PhoneNumber, string Email, string BankAccount);

		internal class Validator : AbstractValidator<UpdateCommand>
		{
			public Validator()
			{
				RuleFor(x => x.Model.Id).NotEmpty().GuidValidator();

				RuleFor(x => x.Model.FirstName)
					.NotEmpty().WithMessage("Name is required.")
					.MinimumLength(2).WithMessage("Name must be longer than 2 characters.")
					.MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

				RuleFor(x => x.Model.LastName)
					.NotEmpty().WithMessage("LastName is required.")
					.MinimumLength(2).WithMessage("LastName must be longer than 2 characters.")
					.MaximumLength(60).WithMessage("LastName must not exceed 60 characters.");

				RuleFor(x => x.Model.BirthOfDate)
					.GreaterThanOrEqualTo(DateTime.Now.AddYears(-100)).WithMessage($"DateOfBirth should at least greater than or equal to {DateTime.Now.AddYears(-100).ToShortDateString()}.")
					.LessThanOrEqualTo(DateTime.Now).WithMessage("DateOfBirth must not be greater than now");

				RuleFor(x => x.Model.Email)
					.NotEmpty().WithMessage("Email address is required.")
					.EmailValidator();

				RuleFor(x => x.Model.BankAccount)
					.NotEmpty().WithMessage("BankAccount is required.")
					.MinimumLength(10).WithMessage("BankAccount must be longer than 10 characters.")
					.MaximumLength(20).WithMessage("BankAccount must not exceed 20 characters.");

				RuleFor(x => x.Model.PhoneNumber)
					.NotEmpty().WithMessage("PhoneNumber is required.")
					.PhoneNumberValidator();
			}
		}

		internal class Handler(IRepository<Customer> repository) : IRequestHandler<UpdateCommand, ResultModel<CustomerDto>>
		{
			public async Task<ResultModel<CustomerDto>> Handle(UpdateCommand request, CancellationToken cancellationToken)
			{
				var updated = await repository.UpdateAsync(
					Customer.Create(
						request.Model.Id,
						request.Model.FirstName,
						request.Model.LastName,
						request.Model.BirthOfDate,
						request.Model.PhoneNumber,
						request.Model.Email,
						request.Model.BankAccount));

				return ResultModel<CustomerDto>.Create(new CustomerDto
				{
					Id = updated.Id,
					FirstName = updated.FirstName,
					LastName = updated.LastName,
					DateOfBirth = updated.DateOfBirth,
					PhoneNumber = updated.PhoneNumber,
					Created = updated.Created,
					Updated = updated.Updated,
					Email = updated.Email,
					BankAccount = updated.BankAccount
				});
			}
		}
	}
}