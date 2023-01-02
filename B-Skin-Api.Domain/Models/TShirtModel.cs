namespace B_Skin_Api.Domain.Models
{
    public class TShirtModel : EntityBase
    {
        public string ModelName { get; set; }
        public string ModelDescription { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
    }
}
