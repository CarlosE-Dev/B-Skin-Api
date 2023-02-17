using B_Skin_Api.Domain.Models;
using B_Skin_Api.Domain.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace B_Skin_Api.Domain.Interfaces
{
    public interface IProviderRepository
    {
        Task<IEnumerable<Provider>> GetAll(bool includeInactives);
        Task<Provider> GetById(long id, bool includeInactives);
        Task<ProviderDTO> Create(Provider entity);
        Task InactivateById(long id);
        Task ReactivateById(long id);
        Task Update(long id, Provider entity);
        Task UpdateImage(long id, string imageUrl);
        Task ExcludePermanently(long id);
    }
}
