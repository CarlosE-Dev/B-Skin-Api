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
        Task<IEnumerable<TShirtModel>> GetAll(bool onlyActives, TShirtFilterModel filter);
        Task<TShirtModel> GetById(long id, bool onlyActives);
        Task InactivateById(long id);
        Task ReactivateById(long id);
        Task<TShirtModel> Create(TShirtModel entity);
        Task Update(long id, TShirtModel entity);
        Task<IEnumerable<TShirtModel>> SearchTShirtsByKeyWords(string querySearch, int resultLimit);
        Task UpdateImage(long id, string imageUrl);
        Task ExcludePermanently(long id);
        Task<IEnumerable<TShirtModel>> TesteBanco();
    }
}
