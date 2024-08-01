using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PatikaMovieStore.BusinessLayer.Abstract;
using PatikaMovieStore.BusinessLayer.Exceptions;
using PatikaMovieStore.DataAccessLayer.Context;
using PatikaMovieStore.DtoLayer.ActorDtos;
using PatikaMovieStore.EntityLayer.Entities;

namespace PatikaMovieStore.BusinessLayer.Concrete
{
    public class ActorService : IActorService
    {
        private readonly MovieStoreContext _context;
        private readonly IMapper _mapper;

        public ActorService(MovieStoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateActorAsync(CreateActorDto createActorDto)
        {
            var actor = _mapper.Map<Actor>(createActorDto);
            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteActorAsync(int id)
        {
            var actor = await _context.Actors.FindAsync(id);

            if (actor == null)
                throw new NotFoundException($"Actor with ID {id} not found !");

            _context.Actors.Remove(actor);
            await _context.SaveChangesAsync();
        }

        public async Task<ActorDto> GetActorByIdAsync(int id)
        {
            var actor = await _context.Actors.FindAsync(id);

            if (actor == null)
                throw new NotFoundException($"Actor with ID {id} not found !");

            return _mapper.Map<ActorDto>(actor);
        }

        public async Task<List<ActorDto>> GetAllActorAsync()
        {
            var actors = await _context.Actors
                .Include(a => a.ActorMovies)
                    .ThenInclude(am => am.Movie)
                    .ToListAsync();

            var mappedList = _mapper.Map<List<ActorDto>>(actors);
            return mappedList;
        }

        public async Task<List<ActorWithMovieNameDto>> GetAllActorWithMovieNameAsync()
        {
            var actors = await _context.Actors
                .Include(a => a.ActorMovies)
                    .ThenInclude(am => am.Movie)
                    .ToListAsync();

            var mappedList = _mapper.Map<List<ActorWithMovieNameDto>>(actors);
            return mappedList;
        }

        public async Task UpdateActorAsync(UpdateActorDto updateActorDto)
        {
            var actor = await _context.Actors.FindAsync(updateActorDto.Id);
            
            if (actor == null)
                throw new NotFoundException($"Actor with ID {updateActorDto.Id} not found !");

            _mapper.Map(updateActorDto, actor);
            await _context.SaveChangesAsync();
        }
    }
}
