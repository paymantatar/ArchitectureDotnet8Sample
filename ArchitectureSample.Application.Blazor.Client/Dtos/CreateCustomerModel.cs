namespace ArchitectureSample.Application.Blazor.Client.Dtos;

public record CreateCustomerModel(string FirstName, string LastName, DateTime BirthOfDate, string PhoneNumber, string Email, string BankAccount);