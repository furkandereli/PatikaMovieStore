namespace PatikaMovieStore.EntityLayer.Entities
{
    public class Actor
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<ActorMovie> ActorMovies { get; set; }
    }
}
