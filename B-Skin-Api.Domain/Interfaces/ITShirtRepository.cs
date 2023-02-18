using B_Skin_Api.Domain.Models;
using B_Skin_Api.Domain.Models.Dtos;
using B_Skin_Api.Domain.Models.Queries.TShirtQueries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace B_Skin_Api.Domain.Interfaces
{
    public interface ITShirtRepository
    {
        Task<IEnumerable<TShirtDTO>> GetAll(GetAllTShirtsQuery query);
        Task<TShirtDTO> GetById(long id, bool onlyActives);
        Task InactivateById(long id);
        Task ReactivateById(long id);
        Task<TShirtDTO> Create(TShirtModel entity, string sizeIds);
        Task Update(TShirtModel entity, string sizeIds);
        Task<IEnumerable<TShirtDTO>> SearchTShirtsByKeyWords(string querySearch, int resultsLimit);
        Task UpdateImage(long id, string imageUrl);
        Task ExcludePermanently(long id);
        Task<IEnumerable<SizeModel>> GetAvaiableSizes(long tshirtId);
    }
}
