namespace PatikaMovieStore.DtoLayer.ActorDtos
{
    public class ActorWithMovieNameDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> MovieNames { get; set; }
    }
}
