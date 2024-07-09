using ArchitectureSample.Domain.Core.Events;

namespace ArchitectureSample.Application.Dtos;

public class CustomerCreatedIntegrationEvent : EventBase
{
	public Guid Id { get; set; }

	public string FirstName { get; set; } = default!;

	public string LastName { get; set; } = default!;

	public DateTime DateOfBirth { get; set; } = default!;

	public string Email { get; set; } = default!;

	public string PhoneNumber { get; set; } = default!;

	public string BankAccount { get; set; } = default!;


	public override void Flatten()
	{
		MetaData.Add("CustomerId", Id);
		MetaData.Add("CustomerFirstName", FirstName);
		MetaData.Add("CustomerLastName", LastName);
		MetaData.Add("CustomerDateOfBirth", DateOfBirth);
		MetaData.Add("CustomerEmail", Email);
		MetaData.Add("CustomerPhoneNumber", PhoneNumber);
		MetaData.Add("CustomerBankAccount", BankAccount);
	}
}