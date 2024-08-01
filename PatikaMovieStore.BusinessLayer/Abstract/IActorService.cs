using PatikaMovieStore.DtoLayer.ActorDtos;

namespace PatikaMovieStore.BusinessLayer.Abstract
{
    public interface IActorService
    {
        Task<List<ActorDto>> GetAllActorAsync();
        Task<List<ActorWithMovieNameDto>> GetAllActorWithMovieNameAsync();
        Task<ActorDto> GetActorByIdAsync(int id);
        Task CreateActorAsync(CreateActorDto createActorDto);
        Task UpdateActorAsync(UpdateActorDto updateActorDto);
        Task DeleteActorAsync(int id);
    }
}
