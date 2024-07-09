namespace ArchitectureSample.Application.Blazor.Client.Dtos;

public record UpdateCustomerModel(Guid Id, string FirstName, string LastName, DateTime BirthOfDate, string PhoneNumber, string Email, string BankAccount);