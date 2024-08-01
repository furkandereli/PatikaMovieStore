using PatikaMovieStore.DtoLayer.OrderDtos;
using PatikaMovieStore.EntityLayer.Entities;

namespace PatikaMovieStore.DtoLayer.CustomerDtos
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "Customer";
        public List<Genre> FavoriteGenres { get; set; }
        public List<Movie> PurchasedMovies { get; set; }
        public List<OrderDto> Orders { get; set; }
    }
}
