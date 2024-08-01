namespace PatikaMovieStore.DtoLayer.MovieDtos
{
    public class UpdateMovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public List<int> GenreId { get; set; }
        public int DirectorId { get; set; }
        public List<int> ActorIds { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}
