using PatikaMovieStore.DtoLayer.AuthDtos;

namespace PatikaMovieStore.BusinessLayer.Abstract
{
    public interface IAuthService
    {
        Task<string> AuthenticateAsync(string email,string password);
    }
}
