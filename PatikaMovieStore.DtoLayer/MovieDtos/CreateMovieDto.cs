namespace PatikaMovieStore.DtoLayer.MovieDtos
{
    public class CreateMovieDto
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public int GenreId { get; set; }
        public int DirectorId { get; set; }
        public List<int>? ActorIds { get; set; }
        public decimal Price { get; set; }
    }
}
