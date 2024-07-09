using ArchitectureSample.Application.Commands;
using FluentAssertions;
using TechTalk.SpecFlow;
using static ArchitectureSample.Application.Commands.CreateCustomer.Command;

namespace ArchitectureSample.Tests.Steps;

[Binding]
public class CustomerSteps
{
	private CreateCustomer.Command? _customerModels;
	private bool _isSaved;

	[Given(@"I have entered the customer details")]
	public void GivenIHaveEnteredTheCustomerDetails(Table table) =>
		_customerModels = table
				.Rows
				.Select(item =>
					new CreateCustomer.Command
					{
						Model = new CreateCustomerModel(
							item["FirstName"],
							item["LastName"],
							DateTime.Parse(item["BirthOfDate"]),
							item["PhoneNumber"],
							item["Email"],
							item["BankAccount"])
					}
				).Single();

	[When(@"I save the customer")]
	public void WhenISaveTheCustomer() =>
		_isSaved = new Validator().Validate(_customerModels!).IsValid;

	[Then(@"the customer details should be (.*)")]
	public void ThenTheCustomerDetailsShouldBeSavedSuccessfully(string expectation)
	{
		switch (expectation)
		{
			case "Saved":
				_isSaved.Should().BeTrue();
				break;
			case "Not Saved":
				_isSaved.Should().BeFalse();
				break;
			default:
				Action action = () => throw new Exception("Invalid expectation specified");
				action.Should().Throw<Exception>().WithMessage("Invalid expectation specified");
				break;
		}
	}
}