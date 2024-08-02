using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using PatikaMovieStore.BusinessLayer.Concrete;
using PatikaMovieStore.BusinessLayer.Exceptions;
using PatikaMovieStore.DataAccessLayer.Context;
using PatikaMovieStore.DtoLayer.DirectorDtos;
using PatikaMovieStore.EntityLayer.Entities;

namespace PatikaMovieStore.Tests.ServiceTests
{
    public class DirectorServiceTests
    {
        private readonly MovieStoreContext _context;
        private readonly DirectorService _directorService;
        private readonly Mock<IMapper> _mockMapper;

        public DirectorServiceTests()
        {
            _mockMapper = new Mock<IMapper>();
            _context = GetInMemoryDbContext();
            _directorService = new DirectorService(_context, _mockMapper.Object);
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
        public async Task CreateDirectorAsync_ShouldAddDirector()
        {
            // Arrange
            var createDirectorDto = new CreateDirectorDto
            {
                FirstName = "Steven",
                LastName = "Spielberg"
            };
            var director = new Director
            {
                FirstName = "Steven",
                LastName = "Spielberg"
            };
            _mockMapper.Setup(m => m.Map<Director>(createDirectorDto)).Returns(director);

            // Act
            await _directorService.CreateDirectorAsync(createDirectorDto);

            // Assert
            var savedDirector = await _context.Directors.FirstOrDefaultAsync(d => d.FirstName == "Steven" && d.LastName == "Spielberg");
            Assert.NotNull(savedDirector);
            Assert.Equal("Steven", savedDirector.FirstName);
            Assert.Equal("Spielberg", savedDirector.LastName);
        }

        [Fact]
        public async Task DeleteDirectorAsync_ShouldRemoveDirector()
        {
            // Arrange
            var director = new Director
            {
                Id = 1,
                FirstName = "Martin",
                LastName = "Scorsese"
            };
            _context.Directors.Add(director);
            await _context.SaveChangesAsync();

            // Act
            await _directorService.DeleteDirectorAsync(director.Id);

            // Assert
            var deletedDirector = await _context.Directors.FindAsync(director.Id);
            Assert.Null(deletedDirector);
        }

        [Fact]
        public async Task DeleteDirectorAsync_ShouldThrowNotFoundException_WhenDirectorNotFound()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _directorService.DeleteDirectorAsync(999));
        }

        [Fact]
        public async Task GetAllDirectorAsync_ShouldReturnDirectorDtos()
        {
            // Arrange
            var directors = new List<Director>
        {
            new Director
            {
                Id = 1,
                FirstName = "Christopher",
                LastName = "Nolan"
            }
        };
            var directorDtos = new List<DirectorDto>
        {
            new DirectorDto
            {
                Id = 1,
                FirstName = "Christopher",
                LastName = "Nolan",
                Movies = new List<Movie>()  // Add as needed
            }
        };
            _context.Directors.AddRange(directors);
            await _context.SaveChangesAsync();
            _mockMapper.Setup(m => m.Map<List<DirectorDto>>(directors)).Returns(directorDtos);

            // Act
            var result = await _directorService.GetAllDirectorAsync();

            // Assert
            Assert.Equal(directorDtos.Count, result.Count);
            Assert.Equal(directorDtos.First().FirstName, result.First().FirstName);
        }

        [Fact]
        public async Task GetDirectorByIdAsync_ShouldReturnDirectorDto()
        {
            // Arrange
            var director = new Director
            {
                Id = 1,
                FirstName = "Quentin",
                LastName = "Tarantino"
            };
            var directorDto = new DirectorDto
            {
                Id = 1,
                FirstName = "Quentin",
                LastName = "Tarantino",
                Movies = new List<Movie>()  // Add as needed
            };
            _context.Directors.Add(director);
            await _context.SaveChangesAsync();
            _mockMapper.Setup(m => m.Map<DirectorDto>(director)).Returns(directorDto);

            // Act
            var result = await _directorService.GetDirectorByIdAsync(director.Id);

            // Assert
            Assert.Equal(directorDto.Id, result.Id);
            Assert.Equal(directorDto.FirstName, result.FirstName);
        }

        [Fact]
        public async Task GetDirectorByIdAsync_ShouldThrowNotFoundException_WhenDirectorNotFound()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _directorService.GetDirectorByIdAsync(999));
        }

        [Fact]
        public async Task UpdateDirectorAsync_ShouldUpdateDirector()
        {
            // Arrange
            var director = new Director
            {
                Id = 1,
                FirstName = "Ridley",
                LastName = "Scott"
            };
            var updateDirectorDto = new UpdateDirectorDto
            {
                Id = 1,
                FirstName = "Ridley",
                LastName = "Scott"
            };
            _context.Directors.Add(director);
            await _context.SaveChangesAsync();
            _mockMapper.Setup(m => m.Map(updateDirectorDto, director)).Verifiable();

            // Act
            await _directorService.UpdateDirectorAsync(updateDirectorDto);

            // Assert
            _mockMapper.Verify();
            var updatedDirector = await _context.Directors.FindAsync(director.Id);
            Assert.Equal(updateDirectorDto.FirstName, updatedDirector.FirstName);
            Assert.Equal(updateDirectorDto.LastName, updatedDirector.LastName);
        }

        [Fact]
        public async Task UpdateDirectorAsync_ShouldThrowNotFoundException_WhenDirectorNotFound()
        {
            // Arrange
            var updateDirectorDto = new UpdateDirectorDto { Id = 999 };

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _directorService.UpdateDirectorAsync(updateDirectorDto));
        }
    }
}
