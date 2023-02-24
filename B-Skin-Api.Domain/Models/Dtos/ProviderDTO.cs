namespace B_Skin_Api.Domain.Models.Dtos
{
    public class ProviderDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Document { get; set; }
        public bool IsActive { get; set; }
        public string ProviderTypeName { get; set; }
        public string ImageUrl { get; set; }
    }
}
