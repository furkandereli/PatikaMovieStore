using PatikaMovieStore.DtoLayer.CustomerDtos;

namespace PatikaMovieStore.BusinessLayer.Abstract
{
    public interface ICustomerService
    {
        Task<List<CustomerDto>> GetAllCustomerAsync();
        Task<CustomerDto> GetCustomerByIdAsync(int id);
        Task CreateCustomerAsync(CreateCustomerDto createCustomerDto);
        Task UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto);
        Task DeleteCustomerAsync(int id);
    }
}
