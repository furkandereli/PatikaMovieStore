using PatikaMovieStore.DtoLayer.DirectorDtos;

namespace PatikaMovieStore.BusinessLayer.Abstract
{
    public interface IDirectorService
    {
        Task<List<DirectorDto>> GetAllDirectorAsync();
        Task<DirectorDto> GetDirectorByIdAsync(int id);
        Task CreateDirectorAsync(CreateDirectorDto createDirectorDto);
        Task UpdateDirectorAsync(UpdateDirectorDto updateDirectorDto);
        Task DeleteDirectorAsync(int id);
    }
}
