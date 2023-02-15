using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace B_Skin_Api.Domain.Models
{
    public class EntityBase
    {
        public EntityBase()
        {
            CreatedOn = DateTime.UtcNow;
        }

        [Key]
        public long Id { get; set; }

        [JsonIgnore]
        public DateTime CreatedOn { get; }
    }
}
