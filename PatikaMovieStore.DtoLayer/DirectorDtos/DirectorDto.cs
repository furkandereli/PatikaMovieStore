using PatikaMovieStore.EntityLayer.Entities;

namespace PatikaMovieStore.DtoLayer.DirectorDtos
{
    public class DirectorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Movie> Movies { get; set; }
    }
}
