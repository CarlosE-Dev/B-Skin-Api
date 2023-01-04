using B_Skin_Api.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace B_Skin_Api.Domain.Interfaces
{
    public interface IProviderRepository
    {
        Task<IEnumerable<Provider>> GetAll(bool includeInactives);
        Task<Provider> GetById(long id, bool includeInactives);
    }
}
