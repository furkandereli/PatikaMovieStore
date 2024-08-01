using PatikaMovieStore.DtoLayer.MovieDtos;

namespace PatikaMovieStore.BusinessLayer.Abstract
{
    public interface IMovieService
    {
        Task<List<MovieDto>> GetAllMovieAsync();
        Task<MovieDto> GetMovieByIdAsync(int id);
        Task CreateMovieAsync(CreateMovieDto createMovieDto);
        Task UpdateMovieAsync(UpdateMovieDto updateMovieDto);
        Task DeleteMovieAsync(int id);
    }
}
