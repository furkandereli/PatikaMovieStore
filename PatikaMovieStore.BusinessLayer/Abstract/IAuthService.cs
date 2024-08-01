using PatikaMovieStore.DtoLayer.AuthDtos;

namespace PatikaMovieStore.BusinessLayer.Abstract
{
    public interface IAuthService
    {
        Task<TokenResponseDto> AuthenticateAsync(string email, string password);
    }
}
