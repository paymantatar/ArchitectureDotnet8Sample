using ArchitectureSample.Application.Blazor.Client.Dtos;

namespace ArchitectureSample.Application.Blazor.Client.Services;

public interface ICustomerService
{
	Task<ApiResponse<Data<CustomerDto>>?> Get(QueryApiRequest apiRequest);

	Task<ApiResponse<Data<CustomerDto>>?> GetById(Guid id);

	Task<ApiResponse<CustomerDto>?> Delete(Guid id);

	Task<ApiResponse<CustomerDto>?> Create(CustomerDto customerDto);

	Task<ApiResponse<CustomerDto>?> Update(CustomerDto customerDto);
}