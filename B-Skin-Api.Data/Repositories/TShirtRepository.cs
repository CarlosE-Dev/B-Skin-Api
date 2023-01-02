using B_Skin_Api.Data.Dapper;
using B_Skin_Api.Domain.Interfaces;
using B_Skin_Api.Domain.Models;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace B_Skin_Api.Data.Repositories
{
    public class TShirtRepository : ITShirtRepository
    {
        private DbSession _session;
        private readonly IUnitOfWork _uow;
        private string _sql;
        public TShirtRepository(DbSession session, IUnitOfWork uow)
        {
            _session = session;
            _uow = uow;
        }

        public async Task<IEnumerable<TShirtModel>> GetAll()
        {
            _sql = $@"
                    SELECT
                        ID                      AS Id,
                        NAME                    AS ModelName,
                        DESCRIPTION             AS ModelDescription,
                        PRICE                   AS Price,
                        QUANTITY_IN_STOCK       AS QuantityInStock,
                        CREATED_ON              AS CreatedOn
                    FROM
                        BS_TSHIRTS
                    ";

            return await _session.Connection.QueryAsync<TShirtModel>(_sql, null, _session.Transaction);
        }
    }
}
