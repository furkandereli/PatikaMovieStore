namespace PatikaMovieStore.DtoLayer.AuthDtos
{
    public class TokenResponseDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
