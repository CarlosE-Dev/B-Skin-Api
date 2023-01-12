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
                        WHERE 1+1
                        ";

            if (onlyActives)
                query += _onlyActivesQuery;

            query += " ORDER BY BSP.NAME";

            var result = await _session.Connection.QueryAsync<Provider>(query, null, _session.Transaction);

            return result;
        }
        public async Task<Provider> GetById(long id, bool onlyActives = true)
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

            var result = await _session.Connection.QueryFirstOrDefaultAsync<Provider>(query, new { id }, _session.Transaction);

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

        public async Task<Provider> Create(Provider entity)
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
                throw new Exception("Provider not found!");

            return createdEntity;
        }

        public async Task Update(long id, Provider entity)
        {
            var currentModel = await GetById(id);
            currentModel.Name = entity.Name;
            currentModel.Description = entity.Description;
            currentModel.Document = entity.Document;
            currentModel.IsActive = entity.IsActive;
            currentModel.Email = entity.Email;
            currentModel.Country = entity.Country;
            currentModel.Phone = entity.Phone;
            currentModel.ProviderTypeId = entity.ProviderTypeId;

            var query = $@"UPDATE BS_PROVIDERS
                            SET 
                                NAME =              @Name, 
                                DESCRIPTION =       @Description, 
                                DOCUMENT =          @Document, 
                                IS_ACTIVE =         @IsActive,
                                COUNTRY =           @Country,
                                EMAIL =             @Email,
                                PHONE =             @Phone,
                                PROVIDER_TYPE =     @ProviderTypeId,
                                IMAGE_URL =         @ImageUrl
                            WHERE ID = @Id
                            ";
            try
            {
                _uow.BeginTransaction();
                await _session.Connection.ExecuteScalarAsync(query, currentModel, _session.Transaction);
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

        private async Task<Provider> GetByName(string name, bool onlyActives = true)
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

            return await _session.Connection.QueryFirstOrDefaultAsync<Provider>(query, new { name }, _session.Transaction);
        }
    }
}
