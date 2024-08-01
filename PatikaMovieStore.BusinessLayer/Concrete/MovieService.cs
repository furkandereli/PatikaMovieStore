using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PatikaMovieStore.BusinessLayer.Abstract;
using PatikaMovieStore.BusinessLayer.Exceptions;
using PatikaMovieStore.DataAccessLayer.Context;
using PatikaMovieStore.DtoLayer.MovieDtos;
using PatikaMovieStore.EntityLayer.Entities;

namespace PatikaMovieStore.BusinessLayer.Concrete
{
    public class MovieService : IMovieService
    {
        private readonly MovieStoreContext _context;
        private readonly IMapper _mapper;

        public MovieService(MovieStoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateMovieAsync(CreateMovieDto createMovieDto)
        {
            var movie = _mapper.Map<Movie>(createMovieDto);
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            foreach (var actorId in createMovieDto.ActorIds)
            {
                var actorMovie = new ActorMovie
                {
                    ActorId = actorId,
                    MovieId = movie.Id
                };

                _context.ActorMovies.Add(actorMovie);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteMovieAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                throw new NotFoundException($"Movie with ID {id} not found !");
            }

            //Soft Delete
            movie.IsActive = false;

            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
        }

        public async Task<List<MovieDto>> GetAllMovieAsync()
        {
            var movies = await _context.Movies
                .Where(m => m.IsActive)
                .ToListAsync();

            return _mapper.Map<List<MovieDto>>(movies);
        }

        public async Task<MovieDto> GetMovieByIdAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
                throw new NotFoundException($"Movie with ID {id} not found !");

            return _mapper.Map<MovieDto>(movie);
        }

        public async Task UpdateMovieAsync(UpdateMovieDto updateMovieDto)
        {
            var movie = await _context.Movies.FindAsync(updateMovieDto.Id);

            if (movie == null)
                throw new NotFoundException($"Movie with ID {updateMovieDto.Id} not found !");

            _mapper.Map(updateMovieDto, movie);
            await _context.SaveChangesAsync();
        }
    }
}
