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
    }
}
