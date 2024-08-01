using PatikaMovieStore.DtoLayer.MovieDtos;

namespace PatikaMovieStore.DtoLayer.ActorDtos
{
    public class ActorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<MovieDto> Movies { get; set; }
    }
}
