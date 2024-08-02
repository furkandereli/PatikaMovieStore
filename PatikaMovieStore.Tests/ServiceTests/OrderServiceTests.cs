using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using PatikaMovieStore.BusinessLayer.Concrete;
using PatikaMovieStore.BusinessLayer.Exceptions;
using PatikaMovieStore.DataAccessLayer.Context;
using PatikaMovieStore.DtoLayer.CustomerDtos;
using PatikaMovieStore.DtoLayer.MovieDtos;
using PatikaMovieStore.DtoLayer.OrderDtos;
using PatikaMovieStore.EntityLayer.Entities;

namespace PatikaMovieStore.Tests.ServiceTests
{
    public class OrderServiceTests
    {
        private readonly MovieStoreContext _context;
        private readonly OrderService _orderService;
        private readonly Mock<IMapper> _mockMapper;

        public OrderServiceTests()
        {
            _mockMapper = new Mock<IMapper>();
            _context = GetInMemoryDbContext();
            _orderService = new OrderService(_context, _mockMapper.Object);
        }

        private MovieStoreContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<MovieStoreContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new MovieStoreContext(options);
            return context;
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldAddOrder()
        {
            // Arrange
            var createOrderDto = new CreateOrderDto
            {
                CustomerId = 1,
                MovieId = 1,
                Price = 14.99m,
                PurchaseDate = DateTime.UtcNow
            };

            var order = new Order
            {
                CustomerId = 1,
                MovieId = 1,
                Price = 14.99m,
                PurchaseDate = createOrderDto.PurchaseDate
            };

            _mockMapper.Setup(m => m.Map<Order>(createOrderDto)).Returns(order);

            // Act
            await _orderService.CreateOrderAsync(createOrderDto);

            // Assert
            var savedOrder = await _context.Orders.FirstOrDefaultAsync(o => o.CustomerId == 1 && o.MovieId == 1);
            Assert.NotNull(savedOrder);
            Assert.Equal(createOrderDto.Price, savedOrder.Price);
        }

        [Fact]
        public async Task DeleteOrderAsync_ShouldRemoveOrder()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                CustomerId = 1,
                MovieId = 1,
                Price = 14.99m,
                PurchaseDate = DateTime.UtcNow
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Act
            await _orderService.DeleteOrderAsync(order.Id);

            // Assert
            var deletedOrder = await _context.Orders.FindAsync(order.Id);
            Assert.Null(deletedOrder);
        }

        [Fact]
        public async Task DeleteOrderAsync_ShouldThrowNotFoundException_WhenOrderNotFound()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _orderService.DeleteOrderAsync(999));
        }

        [Fact]
        public async Task GetAllOrderAsync_ShouldReturnAllOrders()
        {
            // Arrange
            var orders = new List<Order>
        {
            new Order { Id = 1, CustomerId = 1, MovieId = 1, Price = 14.99m, PurchaseDate = DateTime.UtcNow },
            new Order { Id = 2, CustomerId = 2, MovieId = 2, Price = 19.99m, PurchaseDate = DateTime.UtcNow }
        };

            var orderDtos = new List<OrderDto>
        {
            new OrderDto { Id = 1, Customer = new CustomerDto(), Movie = new MovieDto(), Price = 14.99m, PurchaseDate = DateTime.UtcNow },
            new OrderDto { Id = 2, Customer = new CustomerDto(), Movie = new MovieDto(), Price = 19.99m, PurchaseDate = DateTime.UtcNow }
        };

            _context.Orders.AddRange(orders);
            await _context.SaveChangesAsync();
            _mockMapper.Setup(m => m.Map<List<OrderDto>>(orders)).Returns(orderDtos);

            // Act
            var result = await _orderService.GetAllOrderAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(14.99m, result[0].Price);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ShouldReturnOrderDto()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                CustomerId = 1,
                MovieId = 1,
                Price = 14.99m,
                PurchaseDate = DateTime.UtcNow
            };
            var orderDto = new OrderDto
            {
                Id = 1,
                Customer = new CustomerDto(),
                Movie = new MovieDto(),
                Price = 14.99m,
                PurchaseDate = DateTime.UtcNow
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            _mockMapper.Setup(m => m.Map<OrderDto>(order)).Returns(orderDto);

            // Act
            var result = await _orderService.GetOrderByIdAsync(order.Id);

            // Assert
            Assert.Equal(orderDto.Id, result.Id);
            Assert.Equal(orderDto.Price, result.Price);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ShouldThrowNotFoundException_WhenOrderNotFound()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _orderService.GetOrderByIdAsync(999));
        }

        [Fact]
        public async Task UpdateOrderAsync_ShouldUpdateOrder()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                CustomerId = 1,
                MovieId = 1,
                Price = 14.99m,
                PurchaseDate = DateTime.UtcNow
            };

            var updateOrderDto = new UpdateOrderDto
            {
                Id = 1,
                CustomerId = 1,
                MovieId = 1,
                Price = 19.99m,
                PurchaseDate = DateTime.UtcNow
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            _mockMapper.Setup(m => m.Map(updateOrderDto, order)).Verifiable();

            order.Price = updateOrderDto.Price;

            // Act
            await _orderService.UpdateOrderAsync(updateOrderDto);

            // Assert
            _mockMapper.Verify();
            var updatedOrder = await _context.Orders.FindAsync(order.Id);
            Assert.Equal(updateOrderDto.Price, updatedOrder.Price);
        }

        [Fact]
        public async Task UpdateOrderAsync_ShouldThrowNotFoundException_WhenOrderNotFound()
        {
            // Arrange
            var updateOrderDto = new UpdateOrderDto { Id = 999 };

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _orderService.UpdateOrderAsync(updateOrderDto));
        }
    }
}
