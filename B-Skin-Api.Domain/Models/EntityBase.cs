using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
