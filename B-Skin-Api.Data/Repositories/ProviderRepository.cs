using B_Skin_Api.Data.Dapper;
using B_Skin_Api.Domain.Interfaces;
using B_Skin_Api.Domain.Models;
using B_Skin_Api.Domain.Models.Dtos;
using Dapper;
using System;
using System.Collections.Generic;
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
            _onlyActivesQuery = " AND BSP.IS_ACTIVE = 1";
        }

        public async Task<IEnumerable<ProviderDTO>> GetAll(bool onlyActives = true)
        {
            var query = $@"
                        SELECT
                            BSP.ID                      AS Id,
                            BSP.NAME                    AS Name,
                            BSP.DESCRIPTION             AS Description,
                            BSP.DOCUMENT                AS Document,
                            BSP.CREATED_ON              AS CreatedOn,
                            BSP.IS_ACTIVE               AS IsActive,
                            BSP.COUNTRY                 AS Country,
                            BSP.EMAIL                   AS Email,
                            BSP.PHONE                   AS Phone,
                            BSP.PROVIDER_TYPE           AS ProviderTypeId,
                            BSPT.TYPE                   AS ProviderTypeName,
                            BSP.IMAGE_URL               AS ImageUrl
                        FROM
                            BS_PROVIDERS BSP
                        LEFT JOIN BS_PROVIDER_TYPE BSPT
                            ON BSP.PROVIDER_TYPE = BSPT.ID 
                        WHERE 1 = 1
                        ";

            if (onlyActives)
                query += _onlyActivesQuery;

            query += " ORDER BY BSP.NAME";

            var result = await _session.Connection.QueryAsync<ProviderDTO>(query, null, _session.Transaction);

            return result;
        }
        public async Task<ProviderDTO> GetById(long id, bool onlyActives = true)
        {
            var query = $@"
                        SELECT
                            BSP.ID                      AS Id,
                            BSP.NAME                    AS Name,
                            BSP.DESCRIPTION             AS Description,
                            BSP.DOCUMENT                AS Document,
                            BSP.CREATED_ON              AS CreatedOn,
                            BSP.IS_ACTIVE               AS IsActive,
                            BSP.COUNTRY                 AS Country,
                            BSP.EMAIL                   AS Email,
                            BSP.PHONE                   AS Phone,
                            BSP.PROVIDER_TYPE           AS ProviderTypeId,
                            BSPT.TYPE                   AS ProviderTypeName,
                            BSP.IMAGE_URL               AS ImageUrl
                        FROM
                            BS_PROVIDERS BSP
                        LEFT JOIN BS_PROVIDER_TYPE BSPT
                            ON BSP.PROVIDER_TYPE = BSPT.ID 
                        WHERE BSP.ID = @id
                        ";

            if (onlyActives)
                query += _onlyActivesQuery;

            var result = await _session.Connection.QueryFirstOrDefaultAsync<ProviderDTO>(query, new { id }, _session.Transaction);

            if (result == null)
                throw new Exception("Provider not found!");

            return result;
        }

        public async Task InactivateById(long id)
        {
            var query = $@"
                        UPDATE 
                            BS_PROVIDERS
                        SET 
                            IS_ACTIVE = 0
                        WHERE 
                            ID = @id
                        ";

            await GetById(id);

            try
            {
                _uow.BeginTransaction();
                await _session.Connection.ExecuteScalarAsync(query, new { id }, _session.Transaction);
                _uow.Commit();
            }
            catch (Exception e)
            {
                _uow.Rollback();
                throw new Exception(e.Message);
            }
            finally
            {
            }
        }

        public async Task ReactivateById(long id)
        {
            var query = $@"
                        UPDATE 
                            BS_PROVIDERS
                        SET 
                            IS_ACTIVE = 1
                        WHERE 
                            ID = @id
                        ";

            await GetById(id, false);

            try
            {
                _uow.BeginTransaction();
                await _session.Connection.ExecuteScalarAsync(query, new { id }, _session.Transaction);
                _uow.Commit();
            }
            catch (Exception e)
            {
                _uow.Rollback();
                throw new Exception(e.Message);
            }
            finally
            {
                _uow.Dispose();
            }
        }

        public async Task<ProviderDTO> Create(Provider entity)
        {
            var query = $@" INSERT INTO 
                            BS_PROVIDERS
                                (NAME, DESCRIPTION, DOCUMENT, CREATED_ON, IS_ACTIVE, EMAIL, PHONE, PROVIDER_TYPE, COUNTRY, IMAGE_URL)
                            VALUES
                                (@Name, @Description, @Document, @CreatedOn, @IsActive, @Email, @Phone, @ProviderTypeId, @Country, @ImageUrl)
                        ";
            try
            {
                _uow.BeginTransaction();
                await _session.Connection.ExecuteScalarAsync(query, entity, _session.Transaction);
                _uow.Commit();
            }
            catch (Exception e)
            {
                _uow.Rollback();
                throw new Exception(e.Message);
            }
            finally
            {
                _uow.Dispose();
            }

            var createdEntity = await GetByName(entity.Name);

            if (createdEntity == null)
                throw new Exception("An error ocurred during the process.");

            return createdEntity;
        }

        public async Task Update(Provider entity, string providerTypeName)
        {
            var currentModel = await GetById(entity.Id, false);

            if (currentModel == null) throw new Exception("Provider not found.");

            var providerTypeId = await GetProviderTypeIdByName(providerTypeName);

            var query = $@"UPDATE BS_PROVIDERS
                            SET 
                                NAME =              @Name, 
                                DESCRIPTION =       @Description, 
                                DOCUMENT =          @Document, 
                                IS_ACTIVE =         @IsActive,
                                COUNTRY =           @Country,
                                EMAIL =             @Email,
                                PHONE =             @Phone,
                                PROVIDER_TYPE =     {providerTypeId},
                                IMAGE_URL =         @ImageUrl
                            WHERE ID = @Id
                            ";
            try
            {
                _uow.BeginTransaction();
                await _session.Connection.ExecuteScalarAsync(query, entity, _session.Transaction);
                _uow.Commit();
            }
            catch (Exception e)
            {
                _uow.Rollback();
                throw new Exception(e.Message);
            }
            finally
            {
                _uow.Dispose();
            }
        }

        private async Task<long> GetProviderTypeIdByName(string providerTypeName)
        {
            var query = $@"SELECT ID
                            FROM BS_PROVIDER_TYPE
                            WHERE [TYPE] = @ProviderTypeName";

            var result = await _session.Connection.QueryFirstOrDefaultAsync<long>(query, new { providerTypeName }, _session.Transaction);

            if (result == null)
                throw new Exception("Provider Type not found");

            return result;
        }

        public async Task ExcludePermanently(long id)
        {
            var query = "DELETE FROM BS_PROVIDERS WHERE ID = @id";

            try
            {
                _uow.BeginTransaction();
                await _session.Connection.ExecuteScalarAsync(query, new {id}, _session.Transaction);
                _uow.Commit();
            }
            catch (Exception e)
            {
                _uow.Rollback();
                throw new Exception(e.Message);
            }
            finally
            {
                _uow.Dispose();
            }
        }

        public async Task UpdateImage(long id, string imageUrl)
        {
            var idExists = await GetById(id);

            var query = $@"UPDATE BS_PROVIDERS
                            SET IMAGE_URL = @imageUrl
                            WHERE ID = @id
                            ";
            try
            {
                _uow.BeginTransaction();
                await _session.Connection.ExecuteScalarAsync(query, new { id, imageUrl }, _session.Transaction);
                _uow.Commit();
            }
            catch (Exception e)
            {
                _uow.Rollback();
                throw new Exception(e.Message);
            }
            finally
            {
                _uow.Dispose();
            }
        }

        private async Task<ProviderDTO> GetByName(string name, bool onlyActives = true)
        {
            var query = $@"
                        SELECT
                            BSP.ID                      AS Id,
                            BSP.NAME                    AS Name,
                            BSP.DESCRIPTION             AS Description,
                            BSP.DOCUMENT                AS Document,
                            BSP.CREATED_ON              AS CreatedOn,
                            BSP.IS_ACTIVE               AS IsActive,
                            BSP.COUNTRY                 AS Country,
                            BSP.EMAIL                   AS Email,
                            BSP.PHONE                   AS Phone,
                            BSP.PROVIDER_TYPE           AS ProviderTypeId,
                            BSPT.TYPE                   AS ProviderTypeName,
                            BSP.IMAGE_URL               AS ImageUrl
                        FROM
                            BS_PROVIDERS BSP
                        LEFT JOIN BS_PROVIDER_TYPE BSPT
                            ON BSP.PROVIDER_TYPE = BSPT.ID 
                        WHERE BSP.NAME = @name
                        ";

            if (onlyActives)
                query += _onlyActivesQuery;

            return await _session.Connection.QueryFirstOrDefaultAsync<ProviderDTO>(query, new { name }, _session.Transaction);
        }
    }
}
