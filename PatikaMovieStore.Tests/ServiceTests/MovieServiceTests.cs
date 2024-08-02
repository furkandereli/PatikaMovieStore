using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using PatikaMovieStore.BusinessLayer.Concrete;
using PatikaMovieStore.BusinessLayer.Exceptions;
using PatikaMovieStore.DataAccessLayer.Context;
using PatikaMovieStore.DtoLayer.DirectorDtos;
using PatikaMovieStore.DtoLayer.GenreDtos;
using PatikaMovieStore.DtoLayer.MovieDtos;
using PatikaMovieStore.EntityLayer.Entities;

namespace PatikaMovieStore.Tests.ServiceTests
{
    public class MovieServiceTests
    {
        private readonly MovieStoreContext _context;
        private readonly MovieService _movieService;
        private readonly Mock<IMapper> _mockMapper;

        public MovieServiceTests()
        {
            _mockMapper = new Mock<IMapper>();
            _context = GetInMemoryDbContext();
            _movieService = new MovieService(_context, _mockMapper.Object);
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
        public async Task CreateMovieAsync_ShouldAddMovieAndActorMovies()
        {
            // Arrange
            var createMovieDto = new CreateMovieDto
            {
                Title = "Inception",
                Year = 2010,
                GenreId = 1,
                DirectorId = 1,
                ActorIds = new List<int> { 1, 2 },
                Price = 19.99m
            };

            var movie = new Movie
            {
                Title = "Inception",
                Year = 2010,
                GenreId = 1,
                DirectorId = 1,
                Price = 19.99m
            };

            _mockMapper.Setup(m => m.Map<Movie>(createMovieDto)).Returns(movie);

            // Act
            await _movieService.CreateMovieAsync(createMovieDto);

            // Assert
            var savedMovie = await _context.Movies.FirstOrDefaultAsync(m => m.Title == "Inception");
            Assert.NotNull(savedMovie);
            Assert.Equal("Inception", savedMovie.Title);

            var actorMovies = await _context.ActorMovies
                .Where(am => am.MovieId == savedMovie.Id)
                .ToListAsync();
            Assert.Equal(createMovieDto.ActorIds.Count, actorMovies.Count);
        }

        [Fact]
        public async Task DeleteMovieAsync_ShouldSoftDeleteMovie()
        {
            // Arrange
            var movie = new Movie
            {
                Id = 1,
                Title = "The Matrix",
                Year = 1999,
                IsActive = true
            };
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            // Act
            await _movieService.DeleteMovieAsync(movie.Id);

            // Assert
            var deletedMovie = await _context.Movies.FindAsync(movie.Id);
            Assert.NotNull(deletedMovie);
            Assert.False(deletedMovie.IsActive);
        }

        [Fact]
        public async Task DeleteMovieAsync_ShouldThrowNotFoundException_WhenMovieNotFound()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _movieService.DeleteMovieAsync(999));
        }

        [Fact]
        public async Task GetAllMovieAsync_ShouldReturnActiveMovies()
        {
            // Arrange
            var movies = new List<Movie>
        {
            new Movie { Id = 1, Title = "Titanic", Year = 1997, IsActive = true },
            new Movie { Id = 2, Title = "Avatar", Year = 2009, IsActive = false }
        };

            var movieDtos = new List<MovieDto>
        {
            new MovieDto { Id = 1, Title = "Titanic", Year = 1997, IsActive = true }
        };

            _context.Movies.AddRange(movies);
            await _context.SaveChangesAsync();
            _mockMapper.Setup(m => m.Map<List<MovieDto>>(It.IsAny<List<Movie>>())).Returns(movieDtos);

            // Act
            var result = await _movieService.GetAllMovieAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("Titanic", result.First().Title);
        }

        [Fact]
        public async Task GetMovieByIdAsync_ShouldReturnMovieDto()
        {
            // Arrange
            var movie = new Movie
            {
                Id = 1,
                Title = "Interstellar",
                Year = 2014,
                GenreId = 1,
                DirectorId = 1,
                Price = 14.99m,
                IsActive = true
            };
            var movieDto = new MovieDto
            {
                Id = 1,
                Title = "Interstellar",
                Year = 2014,
                Genre = new List<GenreDto>(), // Add as needed
                Director = new DirectorDto(), // Add as needed
                Price = 14.99m,
                IsActive = true
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            _mockMapper.Setup(m => m.Map<MovieDto>(movie)).Returns(movieDto);

            // Act
            var result = await _movieService.GetMovieByIdAsync(movie.Id);

            // Assert
            Assert.Equal(movieDto.Id, result.Id);
            Assert.Equal(movieDto.Title, result.Title);
        }

        [Fact]
        public async Task GetMovieByIdAsync_ShouldThrowNotFoundException_WhenMovieNotFound()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _movieService.GetMovieByIdAsync(999));
        }

        [Fact]
        public async Task UpdateMovieAsync_ShouldUpdateMovie()
        {
            // Arrange
            var movie = new Movie
            {
                Id = 1,
                Title = "Gladiator",
                Year = 2000,
                GenreId = 1,
                DirectorId = 1,
                Price = 19.99m,
                IsActive = true
            };

            var updateMovieDto = new UpdateMovieDto
            {
                Id = 1,
                Title = "Gladiator",
                Year = 2000,
                GenreId = new List<int> { 1 },
                DirectorId = 1,
                ActorIds = new List<int> { 1 },
                Price = 18.99m,
                IsActive = true
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            _mockMapper.Setup(m => m.Map(updateMovieDto, movie)).Verifiable();

            movie.Title = updateMovieDto.Title;
            movie.Year = updateMovieDto.Year;
            movie.DirectorId = updateMovieDto.DirectorId;
            movie.Price = updateMovieDto.Price;
            movie.IsActive = updateMovieDto.IsActive;

            // Act
            await _movieService.UpdateMovieAsync(updateMovieDto);

            // Assert
            _mockMapper.Verify();
            var updatedMovie = await _context.Movies.FindAsync(movie.Id);
            Assert.Equal(updateMovieDto.Price, updatedMovie.Price);
        }

        [Fact]
        public async Task UpdateMovieAsync_ShouldThrowNotFoundException_WhenMovieNotFound()
        {
            // Arrange
            var updateMovieDto = new UpdateMovieDto { Id = 999 };

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _movieService.UpdateMovieAsync(updateMovieDto));
        }
    }
}
