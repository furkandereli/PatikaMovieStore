using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using PatikaMovieStore.BusinessLayer.Concrete;
using PatikaMovieStore.BusinessLayer.Exceptions;
using PatikaMovieStore.DataAccessLayer.Context;
using PatikaMovieStore.DtoLayer.CustomerDtos;
using PatikaMovieStore.DtoLayer.OrderDtos;
using PatikaMovieStore.EntityLayer.Entities;

namespace PatikaMovieStore.Tests.ServiceTests
{
    public class CustomerServiceTests
    {
        private readonly MovieStoreContext _context;
        private readonly CustomerService _customerService;
        private readonly Mock<IMapper> _mockMapper;

        public CustomerServiceTests()
        {
            _mockMapper = new Mock<IMapper>();
            _context = GetInMemoryDbContext();
            _customerService = new CustomerService(_context, _mockMapper.Object);
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
        public async Task CreateCustomerAsync_ShouldAddCustomer()
        {
            // Arrange
            var createCustomerDto = new CreateCustomerDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "Password123"
            };
            var customer = new Customer
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "Password123"
            };
            _mockMapper.Setup(m => m.Map<Customer>(createCustomerDto)).Returns(customer);

            // Act
            await _customerService.CreateCustomerAsync(createCustomerDto);

            // Assert
            var savedCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == "john.doe@example.com");
            Assert.NotNull(savedCustomer);
            Assert.Equal("John", savedCustomer.FirstName);
            Assert.Equal("Doe", savedCustomer.LastName);
            Assert.Equal("john.doe@example.com", savedCustomer.Email);
            Assert.Equal("Password123", savedCustomer.Password);
        }

        [Fact]
        public async Task DeleteCustomerAsync_ShouldRemoveCustomer()
        {
            // Arrange
            var customer = new Customer
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane.doe@example.com",
                Password = "Password123"
            };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Act
            await _customerService.DeleteCustomerAsync(customer.Id);

            // Assert
            var deletedCustomer = await _context.Customers.FindAsync(customer.Id);
            Assert.Null(deletedCustomer);
        }

        [Fact]
        public async Task DeleteCustomerAsync_ShouldThrowNotFoundException_WhenCustomerNotFound()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _customerService.DeleteCustomerAsync(999));
        }

        [Fact]
        public async Task GetAllCustomerAsync_ShouldReturnCustomerDtos()
        {
            // Arrange
            var customers = new List<Customer>
        {
            new Customer
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane.doe@example.com",
                Password = "Password123"
            }
        };
            var customerDtos = new List<CustomerDto>
        {
            new CustomerDto
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane.doe@example.com",
                Password = "Password123",
                Role = "Customer",
                FavoriteGenres = new List<Genre>(),  // Add as needed
                PurchasedMovies = new List<Movie>(),  // Add as needed
                Orders = new List<OrderDto>()  // Add as needed
            }
        };
            _context.Customers.AddRange(customers);
            await _context.SaveChangesAsync();
            _mockMapper.Setup(m => m.Map<List<CustomerDto>>(customers)).Returns(customerDtos);

            // Act
            var result = await _customerService.GetAllCustomerAsync();

            // Assert
            Assert.Equal(customerDtos.Count, result.Count);
            Assert.Equal(customerDtos.First().Email, result.First().Email);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ShouldReturnCustomerDto()
        {
            // Arrange
            var customer = new Customer
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane.doe@example.com",
                Password = "Password123"
            };
            var customerDto = new CustomerDto
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane.doe@example.com",
                Password = "Password123",
                Role = "Customer",
                FavoriteGenres = new List<Genre>(),  // Add as needed
                PurchasedMovies = new List<Movie>(),  // Add as needed
                Orders = new List<OrderDto>()  // Add as needed
            };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            _mockMapper.Setup(m => m.Map<CustomerDto>(customer)).Returns(customerDto);

            // Act
            var result = await _customerService.GetCustomerByIdAsync(customer.Id);

            // Assert
            Assert.Equal(customerDto.Id, result.Id);
            Assert.Equal(customerDto.Email, result.Email);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ShouldThrowNotFoundException_WhenCustomerNotFound()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _customerService.GetCustomerByIdAsync(999));
        }

        [Fact]
        public async Task UpdateCustomerAsync_ShouldUpdateCustomer()
        {
            // Arrange
            var customer = new Customer
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane.doe@example.com",
                Password = "OldPassword123"
            };
            var updateCustomerDto = new UpdateCustomerDto
            {
                Id = 1,
                FirstName = "Janet",
                LastName = "Doe",
                Email = "janet.doe@example.com",
                Password = "NewPassword123",
                Role = "Customer"
            };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            _mockMapper.Setup(m => m.Map(updateCustomerDto, customer)).Verifiable();

            customer.FirstName = updateCustomerDto.FirstName;
            customer.LastName = updateCustomerDto.LastName;
            customer.Email = updateCustomerDto.Email;
            customer.Password = updateCustomerDto.Password;
            customer.Role = updateCustomerDto.Role;

            // Act
            await _customerService.UpdateCustomerAsync(updateCustomerDto);

            // Assert
            _mockMapper.Verify();
            var updatedCustomer = await _context.Customers.FindAsync(customer.Id);
            Assert.Equal(updateCustomerDto.FirstName, updatedCustomer.FirstName);
            Assert.Equal(updateCustomerDto.LastName, updatedCustomer.LastName);
            Assert.Equal(updateCustomerDto.Email, updatedCustomer.Email);
            Assert.Equal(updateCustomerDto.Password, updatedCustomer.Password);
        }

        [Fact]
        public async Task UpdateCustomerAsync_ShouldThrowNotFoundException_WhenCustomerNotFound()
        {
            // Arrange
            var updateCustomerDto = new UpdateCustomerDto { Id = 999 };

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _customerService.UpdateCustomerAsync(updateCustomerDto));
        }
    }
}
