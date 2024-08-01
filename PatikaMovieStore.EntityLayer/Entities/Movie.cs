namespace PatikaMovieStore.EntityLayer.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public int DirectorId { get; set; }
        public Director Director { get; set; }
        public List<Genre> Genres { get; set; }
        public List<ActorMovie> ActorMovies { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
