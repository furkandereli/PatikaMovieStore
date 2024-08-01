using PatikaMovieStore.DtoLayer.ActorMovieDtos;
using PatikaMovieStore.DtoLayer.DirectorDtos;
using PatikaMovieStore.DtoLayer.GenreDtos;

namespace PatikaMovieStore.DtoLayer.MovieDtos
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public List<GenreDto> Genre { get; set; }
        public DirectorDto Director { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}
