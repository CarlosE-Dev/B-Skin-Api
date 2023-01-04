using B_Skin_Api.Data.Dapper;
using B_Skin_Api.Domain.Interfaces;
using B_Skin_Api.Domain.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B_Skin_Api.Data.Repositories
{
    public class ProviderRepository : IProviderRepository
    {
        private DbSession _session;
        private readonly IUnitOfWork _uow;
        private string _onlyActivesQuery;
        public ProviderRepository(DbSession session, IUnitOfWork uow)
        {
            _session = session;
            _uow = uow;
            _onlyActivesQuery = " AND BSP.IS_ACTIVE = TRUE";
        }

        public async Task<IEnumerable<Provider>> GetAll(bool onlyActives = true)
        {
            var query = $@"
                        SELECT
                            ID                      AS Id,
                            NAME                    AS Name,
                            DESCRIPTION             AS Description,
                            DOCUMENT                AS Document,
                            CREATED_ON              AS CreatedOn,
                            IS_ACTIVE               AS IsActive
                        FROM
                            BS_PROVIDERS BSP
                        WHERE 1+1
                        ";

            if (onlyActives)
                query += _onlyActivesQuery;

            var result = await _session.Connection.QueryAsync<Provider>(query, null, _session.Transaction);

            if (!result.Any())
                throw new Exception("The query returned no results.");

            return result;
        }

        public async Task<Provider> GetById(long id, bool onlyActives = true)
        {
            var query = $@"
                        SELECT
                            ID                      AS Id,
                            NAME                    AS Name,
                            DESCRIPTION             AS Description,
                            DOCUMENT                AS Document,
                            CREATED_ON              AS CreatedOn,
                            IS_ACTIVE               AS IsActive
                        FROM
                            BS_PROVIDERS BSP
                        WHERE BSP.ID = @id
                        ";

            if (onlyActives)
                query += _onlyActivesQuery;

            var result = await _session.Connection.QueryFirstOrDefaultAsync<Provider>(query, new { id }, _session.Transaction);

            if (result == null)
                throw new Exception("Provider not found!");

            return result;
        }
    }
}
