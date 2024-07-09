using System.ComponentModel.DataAnnotations;
using ArchitectureSample.Application.Dtos;
using ArchitectureSample.Domain.Core.Entities;

namespace ArchitectureSample.Domain.Entities;

public class Customer : EntityRootBase
{
	[MinLength(5), MaxLength(50), Required]
	public string? FirstName { get; set; }

	[MinLength(5), MaxLength(60), Required]
	public string? LastName { get; set; }

	[DataType(DataType.DateTime), Required]
	public DateTime DateOfBirth { get; set; }

	[DataType(DataType.PhoneNumber), MinLength(10), MaxLength(20), Required]
	public string? PhoneNumber { get; set; }

	[DataType(DataType.EmailAddress), MinLength(10), MaxLength(100), Required]
	public string? Email { get; set; }

	[DataType(DataType.CreditCard), MinLength(10), MaxLength(20), Required]
	public string? BankAccount { get; set; }

	public static Customer Create(string firstName, string lastName, DateTime dateOfBirth, string phoneNumber, string email, string bankAccount) =>
		Create(Guid.NewGuid(), firstName, lastName, dateOfBirth, phoneNumber, email, bankAccount);

	public static Customer Create(Guid id, string firstName, string lastName, DateTime dateOfBirth, string phoneNumber, string email, string bankAccount)
	{
		var customer = new Customer
		{
			Id = id,
			FirstName = firstName,
			LastName = lastName,
			Created = DateTime.Now,
			DateOfBirth = dateOfBirth,
			PhoneNumber = phoneNumber,
			BankAccount = bankAccount,
			Email = email
		};

		customer.AddDomainEvent(new CustomerCreatedIntegrationEvent
		{
			Id = customer.Id,
			FirstName = customer.FirstName,
			LastName = customer.LastName,
			DateOfBirth = customer.DateOfBirth,
			Email = customer.Email,
			BankAccount = customer.BankAccount,
			PhoneNumber = customer.PhoneNumber
		});

		return customer;
	}
}