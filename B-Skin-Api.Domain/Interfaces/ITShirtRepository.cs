using B_Skin_Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_Skin_Api.Domain.Interfaces
{
    public interface ITShirtRepository
    {
        Task<IEnumerable<TShirtModel>> GetAll();
    }
}
