using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using PatikaMovieStore.BusinessLayer.Concrete;
using PatikaMovieStore.BusinessLayer.Token;
using PatikaMovieStore.DataAccessLayer.Context;
using PatikaMovieStore.EntityLayer.Entities;

namespace PatikaMovieStore.Tests.ServiceTests
{
    public class AuthServiceTests
    {
        private readonly MovieStoreContext _context;
        private readonly AuthService _authService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly JwtConfig _jwtConfig;

        public AuthServiceTests()
        {
            _mockMapper = new Mock<IMapper>();
            _context = GetInMemoryDbContext();
            _jwtConfig = new JwtConfig
            {
                Secret = "ThisIsMySecretKeyForJWTAndMovieStoreApi",
                AccessTokenExpiration = 60, // minutes
                Issuer = "MovieStoreApi",
                Audience = "MovieStoreApi"
            };

            var options = Options.Create(_jwtConfig);
            _authService = new AuthService(_context, options, _mockMapper.Object);
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
        public async Task AuthenticateAsync_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var customer = new Customer
            {
                Id = 1,
                FirstName = "Murat",
                LastName = "Yıldız",
                Email = "test@example.com",
                Password = "password123",
                Role = "Customer"
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Act
            var token = await _authService.AuthenticateAsync(customer.Email, customer.Password);

            // Assert
            Assert.NotNull(token);
            Assert.True(token.StartsWith("eyJ")); // Basic check if it starts with JWT prefix
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldReturnNull_WhenCredentialsAreInvalid()
        {
            // Arrange
            var customer = new Customer
            {
                Id = 1,
                FirstName = "Murat",
                LastName = "Yıldız",
                Email = "test@example.com",
                Password = "password123",
                Role = "Customer"
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Act
            var token = await _authService.AuthenticateAsync(customer.Email, "wrongpassword");

            // Assert
            Assert.Null(token);
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Act
            var token = await _authService.AuthenticateAsync("nonexistent@example.com", "password123");

            // Assert
            Assert.Null(token);
        }
    }

}
