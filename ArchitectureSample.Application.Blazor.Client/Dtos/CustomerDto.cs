namespace ArchitectureSample.Application.Blazor.Client.Dtos;

public record CustomerDto
{
	public Guid Id { get; set; }

	public string? FirstName { get; set; } = default!;

	public string? LastName { get; set; } = default!;

	public DateTime? DateOfBirth { get; set; } = default!;

	public string? PhoneNumber { get; set; } = default!;

	public string? Email { get; set; } = default!;

	public string? BankAccount { get; set; } = default!;

	public DateTime Created { get; set; }

	public DateTime? Updated { get; set; }
}