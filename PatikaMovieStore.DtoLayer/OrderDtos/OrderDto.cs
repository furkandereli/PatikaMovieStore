using PatikaMovieStore.DtoLayer.CustomerDtos;
using PatikaMovieStore.DtoLayer.MovieDtos;

namespace PatikaMovieStore.DtoLayer.OrderDtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public CustomerDto Customer { get; set; }
        public MovieDto Movie { get; set; }
        public decimal Price { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
