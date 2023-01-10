using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace B_Skin_Api.Domain.Models
{
    public class TShirtModel : EntityBase
    {
        [Required(ErrorMessage = "The field {0} cannot be empty")]
        [StringLength(100, ErrorMessage = "The length of the field {0} must be {2} to {1} characters", MinimumLength = 3)]
        public string ModelName { get; set; }

        [Required(ErrorMessage = "The field {0} cannot be empty")]
        [StringLength(500, ErrorMessage = "The length of the field {0} must be {2} to {1} characters", MinimumLength = 10)]
        public string ModelDescription { get; set; }

        [Required(ErrorMessage = "The field {0} cannot be empty")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "The field {0} cannot be empty")]
        public int QuantityInStock { get; set; }

        [Required(ErrorMessage = "The field {0} cannot be empty")]
        public bool? IsActive { get; set; }

        [Required(ErrorMessage = "The field {0} cannot be empty")]
        [StringLength(1, ErrorMessage = "The length of the field {0} must be {2} character", MinimumLength = 1)]
        public string Gender { get; set; }

        [Required(ErrorMessage = "The field {0} cannot be empty")]
        [StringLength(50, ErrorMessage = "The length of the field {0} must be {2} to {1} characters", MinimumLength = 2)]
        public string Color { get; set; }

        [Required(ErrorMessage = "The field {0} cannot be empty")]
        [StringLength(5, ErrorMessage = "The length of the field {0} must be {2} to {1} characters", MinimumLength = 1)]
        public string Size { get; set; }

        [Required(ErrorMessage = "The field {0} cannot be empty")]
        public long ProviderId { get; set; }

        public string Brand { get; set; }
    }
}
