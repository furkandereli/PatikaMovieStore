using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using PatikaMovieStore.BusinessLayer.Concrete;
using PatikaMovieStore.BusinessLayer.Exceptions;
using PatikaMovieStore.DataAccessLayer.Context;
using PatikaMovieStore.DtoLayer.ActorDtos;
using PatikaMovieStore.DtoLayer.MovieDtos;
using PatikaMovieStore.EntityLayer.Entities;

namespace PatikaMovieStore.Tests.ServiceTests
{
    public class ActorServiceTests
    {
        private readonly MovieStoreContext _context;
        private readonly ActorService _actorService;
        private readonly Mock<IMapper> _mockMapper;

        public ActorServiceTests()
        {
            _mockMapper = new Mock<IMapper>();
            _context = GetInMemoryDbContext();
            _actorService = new ActorService(_context, _mockMapper.Object);
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
        public async Task CreateActorAsync_ShouldAddActor()
        {
            //Arrange
            var createActorDto = new CreateActorDto
            {
                FirstName = "John",
                LastName = "Doe",
                MovieIds = new List<int> { 1, 2 }
            };

            var actor = new Actor
            {
                FirstName = "John",
                LastName = "Doe",
                ActorMovies = new List<ActorMovie>
                {
                    new ActorMovie { MovieId = 1 },
                    new ActorMovie { MovieId = 2 }
                }
            };

            _mockMapper.Setup(m => m.Map<Actor>(createActorDto)).Returns(actor);

            // Act
            await _actorService.CreateActorAsync(createActorDto);

            // Assert
            var savedActor = await _context.Actors.Include(a => a.ActorMovies).FirstOrDefaultAsync(a => a.FirstName == "John" && a.LastName == "Doe");
            Assert.NotNull(savedActor);
            Assert.Equal(2, savedActor.ActorMovies.Count);
        }

        [Fact]
        public async Task DeleteActorAsync_ShouldRemoveActor()
        {
            // Arrange
            var actor = new Actor
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Doe"
            };
            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();

            // Act
            await _actorService.DeleteActorAsync(actor.Id);

            // Assert
            var deletedActor = await _context.Actors.FindAsync(actor.Id);
            Assert.Null(deletedActor);
        }

        [Fact]
        public async Task DeleteActorAsync_ShouldThrowNotFoundException_WhenActorNotFound()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _actorService.DeleteActorAsync(999));
        }

        [Fact]
        public async Task GetActorByIdAsync_ShouldReturnActorDto()
        {
            // Arrange
            var actor = new Actor
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Doe",
                ActorMovies = new List<ActorMovie>
            {
                new ActorMovie { Movie = new Movie { Title = "Movie1" } },
                new ActorMovie { Movie = new Movie { Title = "Movie2" } }
            }
            };
            var actorDto = new ActorDto
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Doe",
                Movies = new List<MovieDto>
            {
                new MovieDto { Title = "Movie1" },
                new MovieDto { Title = "Movie2" }
            }
            };
            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();
            _mockMapper.Setup(m => m.Map<ActorDto>(actor)).Returns(actorDto);

            // Act
            var result = await _actorService.GetActorByIdAsync(actor.Id);

            // Assert
            Assert.Equal(actorDto.Id, result.Id);
            Assert.Equal(actorDto.FirstName, result.FirstName);
            Assert.Equal(actorDto.LastName, result.LastName);
            Assert.Equal(actorDto.Movies.Count, result.Movies.Count);
        }

        [Fact]
        public async Task GetActorByIdAsync_ShouldThrowNotFoundException_WhenActorNotFound()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _actorService.GetActorByIdAsync(999));
        }

        [Fact]
        public async Task GetAllActorAsync_ShouldReturnActorDtos()
        {
            // Arrange
            var actors = new List<Actor>
        {
            new Actor
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Doe",
                ActorMovies = new List<ActorMovie>
                {
                    new ActorMovie { Movie = new Movie { Title = "Movie1" } }
                }
            }
        };
            var actorDtos = new List<ActorDto>
        {
            new ActorDto
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Doe",
                Movies = new List<MovieDto>
                {
                    new MovieDto { Title = "Movie1" }
                }
            }
        };
            _context.Actors.AddRange(actors);
            await _context.SaveChangesAsync();
            _mockMapper.Setup(m => m.Map<List<ActorDto>>(actors)).Returns(actorDtos);

            // Act
            var result = await _actorService.GetAllActorAsync();

            // Assert
            Assert.Equal(actorDtos.Count, result.Count);
            Assert.Equal(actorDtos.First().FirstName, result.First().FirstName);
        }

        [Fact]
        public async Task GetAllActorWithMovieNameAsync_ShouldReturnActorWithMovieNameDtos()
        {
            // Arrange
            var actors = new List<Actor>
        {
            new Actor
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Doe",
                ActorMovies = new List<ActorMovie>
                {
                    new ActorMovie { Movie = new Movie { Title = "Movie1" } },
                    new ActorMovie { Movie = new Movie { Title = "Movie2" } }
                }
            }
        };
            var actorWithMovieNameDtos = new List<ActorWithMovieNameDto>
        {
            new ActorWithMovieNameDto
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Doe",
                MovieNames = new List<string> { "Movie1", "Movie2" }
            }
        };
            _context.Actors.AddRange(actors);
            await _context.SaveChangesAsync();
            _mockMapper.Setup(m => m.Map<List<ActorWithMovieNameDto>>(actors)).Returns(actorWithMovieNameDtos);

            // Act
            var result = await _actorService.GetAllActorWithMovieNameAsync();

            // Assert
            Assert.Equal(actorWithMovieNameDtos.Count, result.Count);
            Assert.Equal(actorWithMovieNameDtos.First().MovieNames, result.First().MovieNames);
        }

        [Fact]
        public async Task UpdateActorAsync_ShouldUpdateActor()
        {
            // Arrange
            var actor = new Actor
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Doe"
            };
            var updateActorDto = new UpdateActorDto
            {
                Id = 1,
                FirstName = "Janet",
                LastName = "Doe"
            };
            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();
            _mockMapper.Setup(m => m.Map(updateActorDto, actor)).Verifiable();

            actor.FirstName = updateActorDto.FirstName;
            actor.LastName = updateActorDto.LastName;

            // Act
            await _actorService.UpdateActorAsync(updateActorDto);

            // Assert
            _mockMapper.Verify();
            var updatedActor = await _context.Actors.FindAsync(actor.Id);
            Assert.Equal(updateActorDto.FirstName, updatedActor.FirstName);
            Assert.Equal(updateActorDto.LastName, updatedActor.LastName);
        }

        [Fact]
        public async Task UpdateActorAsync_ShouldThrowNotFoundException_WhenActorNotFound()
        {
            // Arrange
            var updateActorDto = new UpdateActorDto { Id = 999 };

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _actorService.UpdateActorAsync(updateActorDto));
        }
    }
}