namespace PatikaMovieStore.DtoLayer.OrderDtos
{
    public class CreateOrderDto
    {
        public int CustomerId { get; set; }
        public int MovieId { get; set; }
        public decimal Price { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
