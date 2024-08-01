using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PatikaMovieStore.BusinessLayer.Abstract;
using PatikaMovieStore.BusinessLayer.Exceptions;
using PatikaMovieStore.DataAccessLayer.Context;
using PatikaMovieStore.DtoLayer.ActorDtos;
using PatikaMovieStore.DtoLayer.OrderDtos;
using PatikaMovieStore.EntityLayer.Entities;

namespace PatikaMovieStore.BusinessLayer.Concrete
{
    public class OrderService : IOrderService
    {
        private readonly MovieStoreContext _context;
        private readonly IMapper _mapper;

        public OrderService(MovieStoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            var order = _mapper.Map<Order>(createOrderDto);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                throw new NotFoundException($"Order with ID {id} not found !");

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task<List<OrderDto>> GetAllOrderAsync()
        {
            var orders = await _context.Orders.ToListAsync();
            return _mapper.Map<List<OrderDto>>(orders);
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                throw new NotFoundException($"Order with ID {id} not found !");

            return _mapper.Map<OrderDto>(order);
        }

        public async Task UpdateOrderAsync(UpdateOrderDto updateOrderDto)
        {
            var order = await _context.Orders.FindAsync(updateOrderDto.Id);

            if (order == null)
                throw new NotFoundException($"Actor with ID {updateOrderDto.Id} not found !");

            _mapper.Map(updateOrderDto, order);
            await _context.SaveChangesAsync();
        }
    }
}
