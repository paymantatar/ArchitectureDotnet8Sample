using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using ArchitectureSample.Application.Commands;
using ArchitectureSample.Application.Dtos;
using ArchitectureSample.Tests.Integration.Dtos;
using ArchitectureSample.Tests.Integration.Helpers;
using FluentAssertions;

namespace ArchitectureSample.Tests.Integration;

[TestCaseOrderer("ArchitectureSample.Tests.Integration.Helpers.CustomTestCaseOrderer", "ArchitectureSample.Tests.Integration")]
public class CustomerTests(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>
{
	[Fact]
	[TestOrder(3)]
	public async Task GetCustomer_ReturnExpectedResult()
	{
		var request = new HttpRequestMessage(HttpMethod.Get, "api/v1/customers");

		request.Headers.Add("x-query", "{}");

		var client = factory.CreateDefaultClient();

		var response = await client.SendAsync(request);

		response.StatusCode.Should().Be(HttpStatusCode.OK);
		response.Content.Headers.ContentLength.Should().BeGreaterThan(0);

		var result = JsonSerializer.Deserialize<QueryApiResponse<CustomerDto>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		});

		result.Should().NotBeNull();
		result!.Data!.TotalItems.Should().BeGreaterThan(1);
		result.Data.Items!.Where(x => x.FirstName == "Mahyar").Should()
			.NotBeNull();
		result.IsError.Should().BeFalse();
		result.ErrorMessage.Should().BeNull();
	}

	[Theory]
	[MemberData(nameof(CreateData))]
	[TestOrder(1)]
	public async Task CreateCustomer_ReturnExpectedResult(CreateCustomer.Command.CreateCustomerModel model, HttpStatusCode expectedStatusCode, string validationMessage)
	{
		var client = factory.CreateDefaultClient();

		var response = await client.PostAsJsonAsync("api/v1/customers", new { Model = model });

		response.StatusCode.Should().Be(expectedStatusCode);
		response.Content.Headers.ContentLength.Should().BeGreaterThan(0);

		if (expectedStatusCode == HttpStatusCode.OK)
		{
			var result = JsonSerializer.Deserialize<CommandApiResponse<CustomerDto>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});

			result.Should().NotBeNull();

			result!.Data!.FirstName.Should().Be(model.FirstName);
			result.IsError.Should().BeFalse();
			result.ErrorMessage.Should().BeNull();
		}
		else
		{
			var stringResult = await response.Content.ReadAsStringAsync();

			stringResult.Should().Contain(validationMessage);
		}
	}

	[Theory]
	[MemberData(nameof(UpdateData))]
	[TestOrder(2)]
	public async Task UpdateCustomer_ReturnExpectedResult(UpdateCustomer.UpdateCommand.UpdateCustomerModel model, HttpStatusCode expectedStatusCode, string validationMessage)
	{
		var createResponse = await CreateSampleData();

		var client = factory.CreateDefaultClient();

		var response = await client.PutAsJsonAsync("api/v1/customers",
			new
			{
				Model = expectedStatusCode == HttpStatusCode.OK
					? new UpdateCustomer.UpdateCommand.UpdateCustomerModel
					(
						createResponse!.Data!.Id,
						Constants.ValidSampleFirstName,
						Constants.SampleUpdatedLastName,
						Constants.ValidSampleBirthOfDate,
						Constants.ValidSamplePhoneNumber,
						Constants.ValidSampleEmail,
						Constants.ValidSampleBankAccount
					)
					: model,
			});

		response.StatusCode.Should().Be(expectedStatusCode);
		response.Content.Headers.ContentLength.Should().BeGreaterThan(0);

		if (expectedStatusCode == HttpStatusCode.OK)
		{
			var result = JsonSerializer.Deserialize<CommandApiResponse<CustomerDto>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});

			result.Should().NotBeNull();

			result!.Data!.LastName.Should().Be(model.LastName);
			result.IsError.Should().BeFalse();
			result.ErrorMessage.Should().BeNull();
		}
		else
		{
			var stringResult = await response.Content.ReadAsStringAsync();

			stringResult.Should().Contain(validationMessage);
		}
	}

	[Fact]
	[TestOrder(4)]
	public async Task DeleteCustomer_ReturnExpectedResult()
	{
		var createResponse = await CreateSampleData();

		var client = factory.CreateDefaultClient();

		var request = new HttpRequestMessage(HttpMethod.Delete, "api/v1/customers");

		request.Content = new StringContent(JsonSerializer.Serialize(new { Model = new { createResponse!.Data!.Id } }), new MediaTypeHeaderValue("application/json"));

		var response = await client.SendAsync(request);

		response.StatusCode.Should().Be(HttpStatusCode.OK);
		response.Content.Headers.ContentLength.Should().BeGreaterThan(0);

		var result = JsonSerializer.Deserialize<CommandApiResponse<CustomerDto>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		});

		result.Should().NotBeNull();
		result!.Data!.Id.Should().Be(createResponse!.Data!.Id);
		result.Data.FirstName.Should().Be(Constants.ValidSampleFirstName);
		result.IsError.Should().BeFalse();
		result.ErrorMessage.Should().BeNull();
	}

	private async Task<CommandApiResponse<CustomerDto>?> CreateSampleData()
	{
		var client = factory.CreateDefaultClient();

		var response = await client.PostAsJsonAsync("api/v1/customers", new
		{
			Model = new CreateCustomer.Command.CreateCustomerModel
			(
				Constants.ValidSampleFirstName,
				Constants.ValidSampleLastName,
				Constants.ValidSampleBirthOfDate,
				Constants.ValidSamplePhoneNumber,
				Constants.ValidSampleEmail,
				Constants.ValidSampleBankAccount
			)
		});

		return JsonSerializer.Deserialize<CommandApiResponse<CustomerDto>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		});
	}

	public static List<object[]> CreateData() =>
		new()
		{
			// Valid Request
			new object[]
			{
				new CreateCustomer.Command.CreateCustomerModel
				(
					Constants.ValidSampleFirstName,
					Constants.ValidSampleLastName,
					Constants.ValidSampleBirthOfDate,
					Constants.ValidSamplePhoneNumber,
					Constants.ValidSampleEmail,
					Constants.ValidSampleBankAccount
				),
				HttpStatusCode.OK,
				string.Empty
			},
			// Invalid Phone Number
			new object[]
			{
				new CreateCustomer.Command.CreateCustomerModel
				(
					Constants.ValidSampleFirstName,
					Constants.ValidSampleLastName,
					Constants.ValidSampleBirthOfDate,
					Constants.InvalidSamplePhoneNumber,
					Constants.ValidSampleEmail,
					Constants.ValidSampleBankAccount
				),
				HttpStatusCode.BadRequest,
				"Model Phone Number must be valid phone number."
			},
			// Invalid Email
			new object[]
			{
				new CreateCustomer.Command.CreateCustomerModel
				(
					Constants.ValidSampleFirstName,
					Constants.ValidSampleLastName,
					Constants.ValidSampleBirthOfDate,
					Constants.ValidSamplePhoneNumber,
					Constants.InvalidSampleEmail,
					Constants.ValidSampleBankAccount
				),
				HttpStatusCode.BadRequest,
				"Model Email must be valid email address."
			},
			// Invalid Date of Birth
			new object[]
			{
				new CreateCustomer.Command.CreateCustomerModel
				(
					Constants.ValidSampleFirstName,
					Constants.ValidSampleLastName,
					Constants.InvalidSampleBirthOfDate,
					Constants.ValidSamplePhoneNumber,
					Constants.ValidSampleEmail,
					Constants.ValidSampleBankAccount
				),
				HttpStatusCode.BadRequest,
				$"DateOfBirth should at least greater than or equal to {DateTime.Now.AddYears(-100).ToShortDateString()}."
			},
			// ToDo : Add more tests to cover all validations
		};

	public static List<object[]> UpdateData() =>
		new()
		{
			// Valid Request
			new object[]
			{
				new UpdateCustomer.UpdateCommand.UpdateCustomerModel
				(
					Constants.SampleId,
					Constants.ValidSampleFirstName,
					Constants.SampleUpdatedLastName,
					Constants.ValidSampleBirthOfDate,
					Constants.ValidSamplePhoneNumber,
					Constants.ValidSampleEmail,
					Constants.ValidSampleBankAccount
				),
				HttpStatusCode.OK,
				string.Empty
			},
			// Invalid Phone Number
			new object[]
			{
				new UpdateCustomer.UpdateCommand.UpdateCustomerModel
				(
					Constants.SampleId,
					Constants.ValidSampleFirstName,
					Constants.ValidSampleLastName,
					Constants.ValidSampleBirthOfDate,
					Constants.InvalidSamplePhoneNumber,
					Constants.ValidSampleEmail,
					Constants.ValidSampleBankAccount
				),
				HttpStatusCode.BadRequest,
				"Model Phone Number must be valid phone number."
			},
			// Invalid Email
			new object[]
			{
				new UpdateCustomer.UpdateCommand.UpdateCustomerModel
				(
					Constants.SampleId,
					Constants.ValidSampleFirstName,
					Constants.ValidSampleLastName,
					Constants.ValidSampleBirthOfDate,
					Constants.ValidSamplePhoneNumber,
					Constants.InvalidSampleEmail,
					Constants.ValidSampleBankAccount
				),
				HttpStatusCode.BadRequest,
				"Model Email must be valid email address."
			},
			// Invalid Date of Birth
			new object[]
			{
				new UpdateCustomer.UpdateCommand.UpdateCustomerModel
				(
					Constants.SampleId,
					Constants.ValidSampleFirstName,
					Constants.ValidSampleLastName,
					Constants.InvalidSampleBirthOfDate,
					Constants.ValidSamplePhoneNumber,
					Constants.ValidSampleEmail,
					Constants.ValidSampleBankAccount
				),
				HttpStatusCode.BadRequest,
				$"DateOfBirth should at least greater than or equal to {DateTime.Now.AddYears(-100).ToShortDateString()}."
			},
			// ToDo : Add more tests to cover all validations
		};
}