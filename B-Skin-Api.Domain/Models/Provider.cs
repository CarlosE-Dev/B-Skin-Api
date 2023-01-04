using System.ComponentModel.DataAnnotations;

namespace B_Skin_Api.Domain.Models
{
    public class Provider : EntityBase
    {
        [Required(ErrorMessage = "The field {0} cannot be empty")]
        [StringLength(50, ErrorMessage = "The length of the field {0} must be {2} to {1} characters", MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "The field {0} cannot be empty")]
        [StringLength(200, ErrorMessage = "The length of the field {0} must be {2} to {1} characters", MinimumLength = 10)]
        public string Description { get; set; }

        [Required(ErrorMessage = "The field {0} cannot be empty")]
        [StringLength(14, ErrorMessage = "The length of the field {0} must be {1} characters", MinimumLength = 14)]
        public string Document { get; set; }

        [Required(ErrorMessage = "The field {0} cannot be empty")]
        public bool? IsActive { get; set; }
    }
}
