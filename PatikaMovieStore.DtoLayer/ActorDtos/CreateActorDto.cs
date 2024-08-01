namespace PatikaMovieStore.DtoLayer.ActorDtos
{
    public class CreateActorDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<int>? MovieIds { get; set; }
    }
}
