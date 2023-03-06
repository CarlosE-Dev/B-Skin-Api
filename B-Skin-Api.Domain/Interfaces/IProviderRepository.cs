using B_Skin_Api.Domain.Models;
using B_Skin_Api.Domain.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace B_Skin_Api.Domain.Interfaces
{
    public interface IProviderRepository
    {
        Task<IEnumerable<ProviderDTO>> GetAll(bool includeInactives);
        Task<ProviderDTO> GetById(long id, bool includeInactives);
        Task<ProviderDTO> Create(Provider entity);
        Task InactivateById(long id);
        Task ReactivateById(long id);
        Task Update(Provider entity, string providerTypeName);
        Task UpdateImage(long id, string imageUrl);
        Task ExcludePermanently(long id);
    }
}
