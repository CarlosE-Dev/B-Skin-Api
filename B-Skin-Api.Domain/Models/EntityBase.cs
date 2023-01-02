using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_Skin_Api.Domain.Models
{
    public class EntityBase
    {
        public EntityBase()
        {
            CreatedOn = DateTime.UtcNow;
        }
        public long Id { get; set; }
        public DateTime CreatedOn { get; }
    }
}
