using System.Text;
using System.Text.Json;
using ArchitectureSample.Application.Blazor.Server.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ArchitectureSample.Application.Blazor.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController(IEnumerable<HttpClient> httpClients) : ControllerBase
{
	private readonly HttpClient _httpClient = httpClients.Single(x => x.BaseAddress!.Port != 9090);

	[HttpGet]
	public async Task<IActionResult> Get([FromHeader(Name = "x-query")] string query)
	{
		var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/customers");

		request.Headers.Add("x-query", query);

		var response = await _httpClient.SendAsync(request);

		return response.IsSuccessStatusCode
			? Ok(await response.Content.ReadAsStringAsync())
			: BadRequest(await response.Content.ReadAsStringAsync());
	}

	[HttpPost]
	public async Task<IActionResult> Create(CreateCustomerDto customer)
	{
		var response = await _httpClient.PostAsJsonAsync("api/v1/customers", customer);

		var content = await response.Content.ReadAsStringAsync();

		return response.IsSuccessStatusCode
			? Ok(content)
			: BadRequest(content);
	}

	[HttpPut]
	public async Task<IActionResult> Update(UpdateCustomerDto customer)
	{
		var response = await _httpClient.PutAsJsonAsync("api/v1/customers", customer);

		var content = await response.Content.ReadAsStringAsync();

		return response.IsSuccessStatusCode
			? Ok(content)
			: BadRequest(content);
	}

	[HttpDelete]
	public async Task<IActionResult> Delete([FromBody] DeleteCustomerDto deleteCustomer)
	{
		var request = new HttpRequestMessage(HttpMethod.Delete, "/api/v1/customers");

		var content = new StringContent(JsonSerializer.Serialize(deleteCustomer), Encoding.UTF8, "application/json");

		request.Content = content;

		var response = await _httpClient.SendAsync(request);

		return response.IsSuccessStatusCode
			? Ok(await response.Content.ReadAsStringAsync())
			: BadRequest(await response.Content.ReadAsStringAsync());
	}
}