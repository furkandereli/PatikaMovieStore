using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PatikaMovieStore.BusinessLayer.Abstract;
using PatikaMovieStore.BusinessLayer.Exceptions;
using PatikaMovieStore.DataAccessLayer.Context;
using PatikaMovieStore.DtoLayer.DirectorDtos;
using PatikaMovieStore.EntityLayer.Entities;

namespace PatikaMovieStore.BusinessLayer.Concrete
{
    public class DirectorService : IDirectorService
    {
        private readonly MovieStoreContext _context;
        private readonly IMapper _mapper;

        public DirectorService(MovieStoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateDirectorAsync(CreateDirectorDto createDirectorDto)
        {
            var director = _mapper.Map<Director>(createDirectorDto);
            _context.Directors.Add(director);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDirectorAsync(int id)
        {
            var director = await _context.Directors.FindAsync(id);

            if (director == null)
                throw new NotFoundException($"Director with ID {id} not found !");

            _context.Directors.Remove(director);
            await _context.SaveChangesAsync();
        }

        public async Task<List<DirectorDto>> GetAllDirectorAsync()
        {
            var directors = await _context.Directors.ToListAsync();
            return _mapper.Map<List<DirectorDto>>(directors);
        }

        public async Task<DirectorDto> GetDirectorByIdAsync(int id)
        {
            var director = await _context.Directors.FindAsync(id);

            if (director == null)
                throw new NotFoundException($"Director with ID {id} not found !");

            return _mapper.Map<DirectorDto>(director);
        }

        public async Task UpdateDirectorAsync(UpdateDirectorDto updateDirectorDto)
        {
            var director = await _context.Directors.FindAsync(updateDirectorDto.Id);

            if (director == null)
                throw new NotFoundException($"Director with ID {updateDirectorDto.Id} not found !");

            _mapper.Map(updateDirectorDto, director);
            await _context.SaveChangesAsync();
        }
    }
}
