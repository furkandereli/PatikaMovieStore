namespace PatikaMovieStore.EntityLayer.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "Customer";
        public List<Order> Orders { get; set; }
        public List<Genre> FavoriteGenres { get; set; }
    }
}
