using System.Collections.Generic;

namespace B_Skin_Api.Domain.Models.Dtos
{
    public class TShirtDTO
    {
        public long Id { get; set; }
        public long ProviderId { get; set; }
        public string ModelName { get; set; }
        public string ModelDescription { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public bool? IsActive { get; set; }
        public string Gender { get; set; }
        public string Color { get; set; }
        public string Brand { get; set; }
        public string ImageUrl { get; set; }
        public IEnumerable<SizeModel> AvaiableSizes { get; set; }
    }
}
