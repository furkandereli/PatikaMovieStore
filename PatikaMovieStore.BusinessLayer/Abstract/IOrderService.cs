using PatikaMovieStore.DtoLayer.OrderDtos;

namespace PatikaMovieStore.BusinessLayer.Abstract
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetAllOrderAsync();
        Task<OrderDto> GetOrderByIdAsync(int id);
        Task CreateOrderAsync(CreateOrderDto createOrderDto);
        Task UpdateOrderAsync(UpdateOrderDto updateOrderDto);
        Task DeleteOrderAsync(int id);
    }
}
