using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ArchitectureSample.Application.Blazor.Client.Dtos;

namespace ArchitectureSample.Application.Blazor.Client.Services;

public class CustomerService(IEnumerable<HttpClient> httpClients) : ICustomerService
{
	private readonly HttpClient _httpClient = httpClients.Single(x => x.BaseAddress!.Port != 8080);

	private readonly JsonSerializerOptions _options = new()
	{
		PropertyNameCaseInsensitive = true
	};


	public async Task<ApiResponse<Data<CustomerDto>>?> Get(QueryApiRequest apiRequest)
	{
		var request = new HttpRequestMessage(HttpMethod.Get, "api/Customer");

		request.Headers.Add("x-query", JsonSerializer.Serialize(apiRequest));

		var response = await _httpClient.SendAsync(request);

		return response.IsSuccessStatusCode
			? JsonSerializer.Deserialize<ApiResponse<Data<CustomerDto>>>(
				await response.Content.ReadAsStringAsync(), _options)
			: null;
	}

	public async Task<ApiResponse<Data<CustomerDto>>?> GetById(Guid id)
	{
		var request = new HttpRequestMessage(HttpMethod.Get, "api/Customer");

		request.Headers.Add("x-query", JsonSerializer.Serialize(new QueryApiRequest
		{
			Filters = [new FilterModel("Id", "==", id.ToString())]
		}));

		var response = await _httpClient.SendAsync(request);

		if (response.IsSuccessStatusCode)
		{
			return JsonSerializer.Deserialize<ApiResponse<Data<CustomerDto>>>(await response.Content.ReadAsStringAsync(), _options);
		}
		return null;
	}

	public async Task<ApiResponse<CustomerDto>?> Delete(Guid id)
	{
		var request = new HttpRequestMessage(HttpMethod.Delete, $"api/Customer/");

		var content = new StringContent(JsonSerializer.Serialize(new
		{
			Model = new DeleteCustomerModel(id)
		}), Encoding.UTF8, "application/json");

		request.Content = content;

		var response = await _httpClient.SendAsync(request);

		return response.IsSuccessStatusCode
			? JsonSerializer.Deserialize<ApiResponse<CustomerDto>>(
				await response.Content.ReadAsStringAsync(), _options)
			: new ApiResponse<CustomerDto>
			{
				IsError = true,
				ErrorMessage = await response.Content.ReadAsStringAsync()
			};
	}

	public async Task<ApiResponse<CustomerDto>?> Create(CustomerDto customerDto)
	{
		var response = await _httpClient.PostAsJsonAsync("api/Customer", new
		{
			Model = new CreateCustomerModel(customerDto.FirstName!, customerDto.LastName!, customerDto.DateOfBirth!.Value, customerDto.PhoneNumber!, customerDto.Email!, customerDto.BankAccount!)
		});

		if (response.IsSuccessStatusCode)
		{
			return JsonSerializer.Deserialize<ApiResponse<CustomerDto>>(await response.Content.ReadAsStringAsync(), _options);
		}
		return new ApiResponse<CustomerDto>
		{
			IsError = true,
			ErrorMessage = await response.Content.ReadAsStringAsync()
		};
	}

	public async Task<ApiResponse<CustomerDto>?> Update(CustomerDto customerDto)
	{
		var response = await _httpClient.PutAsJsonAsync("api/Customer", new
		{
			Model = new UpdateCustomerModel(customerDto.Id, customerDto.FirstName!, customerDto.LastName!, customerDto.DateOfBirth!.Value, customerDto.PhoneNumber!, customerDto.Email!, customerDto.BankAccount!)
		});

		if (response.IsSuccessStatusCode)
		{
			return JsonSerializer.Deserialize<ApiResponse<CustomerDto>>(await response.Content.ReadAsStringAsync(), _options);
		}
		return new ApiResponse<CustomerDto>
		{
			IsError = true,
			ErrorMessage = await response.Content.ReadAsStringAsync()
		};
	}
}