﻿using B_Skin_Api.Data.Dapper;
using B_Skin_Api.Domain.Enums;
using B_Skin_Api.Domain.Interfaces;
using B_Skin_Api.Domain.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace B_Skin_Api.Data.Repositories
{
    public class TShirtsRepository : ITShirtRepository
    {
        private DbSession _session;
        private readonly IUnitOfWork _uow;
        private string _onlyActivesQuery;
        public TShirtsRepository(DbSession session, IUnitOfWork uow)
        {
            _session = session;
            _uow = uow;
            _onlyActivesQuery = " AND BSTS.IS_ACTIVE = 1";
        }

        public async Task<IEnumerable<TShirtModel>> GetAll(bool onlyActives = true, TShirtFilterModel filter = null)
        {
            string orderBy = "";

            var query = $@"
                        SELECT
                            BSTS.ID                      AS Id,
                            BSTS.NAME                    AS ModelName,
                            BSTS.DESCRIPTION             AS ModelDescription,
                            BSTS.PRICE                   AS Price,
                            BSTS.QUANTITY_IN_STOCK       AS QuantityInStock,
                            BSTS.CREATED_ON              AS CreatedOn,
                            BSTS.IS_ACTIVE               AS IsActive,
                            BSTS.SIZE                    AS Size,
                            BSTS.PROVIDER_ID             AS ProviderId,
                            BSTS.COLOR                   AS Color,
                            BSTS.GENDER                  AS Gender,
                            BSP.NAME                     AS Brand,
                            BSTS.IMAGE_URL               AS ImageUrl
                        FROM
                            BS_TSHIRTS BSTS
                        LEFT JOIN 
                            BS_PROVIDERS BSP 
                                ON 
                            BSTS.PROVIDER_ID = BSP.ID
                        WHERE 1 = 1
                        ";

            if (onlyActives)
                query += _onlyActivesQuery;

            if (filter.InitialPrice != null && filter.FinalPrice != null && filter.FinalPrice > 0)
            {
                query += $@" AND BSTS.PRICE BETWEEN {filter.InitialPrice} and {filter.FinalPrice}";
            }

            if (filter.FilterType == EFilterTShirt.Brand)
            {
                if (filter.ProviderId != null)
                {
                    query += $@" AND BSTS.PROVIDER_ID = {filter.ProviderId}";
                }
            }

            if (filter.FilterType == EFilterTShirt.Size)
            {
                if (filter.Size != null && filter.Size > 0)
                {
                    string size = "";
                    switch (filter.Size)
                    {
                        case ESizeModel.XS: 
                            size = "XS"; 
                                break;
                        case ESizeModel.S: 
                            size = "S"; 
                                break;
                        case ESizeModel.M: 
                            size = "M"; 
                                break;
                        case ESizeModel.L: 
                            size = "L";
                                break;
                        case ESizeModel.XL: 
                            size = "XL";
                                break;
                    }

                    if (!string.IsNullOrEmpty(size))
                    {
                        query += $@" AND BSTS.SIZE = '{size}'";
                    }
                }
            }

            if (filter.OrderBy.HasValue && filter.OrderBy.Value > 0)
            {
                if (filter.OrderBy == EOrderBy.HighPrice)
                    orderBy = " ORDER BY PRICE DESC";

                if (filter.OrderBy == EOrderBy.LowPrice)
                    orderBy = " ORDER BY PRICE";
            }

            if (string.IsNullOrEmpty(orderBy))
                orderBy = "  ORDER BY BSTS.NAME";

            query += orderBy;

            TShirtFilterModel paginationFilter = null;

            if (!filter.IgnorePagination)
            {
                paginationFilter = new TShirtFilterModel(filter.Page - 1, filter.PageSize, null);
                query += $@" OFFSET ({paginationFilter.Offset}) ROWS FETCH FIRST {filter.PageSize} ROWS ONLY";
            }

            var result = await _session.Connection.QueryAsync<TShirtModel>(query, null, _session.Transaction);

            return result;
        }
        public async Task<IEnumerable<TShirtModel>> SearchTShirtsByKeyWords(string querySearch, int resultLimit)
        {
            var query = $@"
                        SELECT TOP {resultLimit}
                            BSTS.ID                      AS Id,
                            BSTS.NAME                    AS ModelName,
                            BSTS.DESCRIPTION             AS ModelDescription,
                            BSTS.PRICE                   AS Price,
                            BSTS.QUANTITY_IN_STOCK       AS QuantityInStock,
                            BSTS.CREATED_ON              AS CreatedOn,
                            BSTS.IS_ACTIVE               AS IsActive,
                            BSTS.SIZE                    AS Size,
                            BSTS.PROVIDER_ID             AS ProviderId,
                            BSTS.COLOR                   AS Color,
                            BSTS.GENDER                  AS Gender,
                            BSP.NAME                     AS Brand,
                            BSTS.IMAGE_URL               AS ImageUrl
                        FROM
                            BS_TSHIRTS BSTS
                        LEFT JOIN 
                            BS_PROVIDERS BSP 
                                ON 
                            BSTS.PROVIDER_ID = BSP.ID
                            WHERE BSTS.NAME LIKE '{ "%" + querySearch + "%" }'
                            AND BSTS.IS_ACTIVE = 1
                            ORDER BY CHARINDEX( '{ querySearch }', BSTS.NAME )
                        ";

            var result = await _session.Connection.QueryAsync<TShirtModel>(query, null, _session.Transaction);

            return result;
        }

        public async Task<TShirtModel> GetById(long id, bool onlyActives = true)
        { 

            var query = $@"
                        SELECT
                            BSTS.ID                      AS Id,
                            BSTS.NAME                    AS ModelName,
                            BSTS.DESCRIPTION             AS ModelDescription,
                            BSTS.PRICE                   AS Price,
                            BSTS.QUANTITY_IN_STOCK       AS QuantityInStock,
                            BSTS.CREATED_ON              AS CreatedOn,
                            BSTS.IS_ACTIVE               AS IsActive,
                            BSTS.SIZE                  AS Size,
                            BSTS.PROVIDER_ID             AS ProviderId,
                            BSTS.COLOR                   AS Color,
                            BSTS.GENDER                  AS Gender,
                            BSP.NAME                     AS Brand,
                            BSTS.IMAGE_URL               AS ImageUrl
                        FROM
                            BS_TSHIRTS BSTS
                        LEFT JOIN 
                            BS_PROVIDERS BSP 
                                ON 
                            BSTS.PROVIDER_ID = BSP.ID
                        WHERE BSTS.ID = @id
                        ";
                
            if (onlyActives)
                query += _onlyActivesQuery;

            var result = await _session.Connection.QueryFirstOrDefaultAsync<TShirtModel>(query, new {id}, _session.Transaction);

            if (result == null)
                throw new Exception("T-Shirt not found!");

            return result;
        }

        public async Task InactivateById(long id)
        {
            var query = $@"
                        UPDATE 
                            BS_TSHIRTS
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
            catch(Exception e)
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
                            BS_TSHIRTS
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

        public async Task<TShirtModel> Create(TShirtModel entity)
        {
            var query = $@" INSERT INTO 
                            BS_TSHIRTS
                                (NAME, DESCRIPTION, PRICE, QUANTITY_IN_STOCK, CREATED_ON, IS_ACTIVE, COLOR, PROVIDER_ID, SIZE, GENDER, IMAGE_URL)
                            VALUES
                                (@ModelName, @ModelDescription, @Price, @QuantityInStock, @CreatedOn, @IsActive, @Color, @ProviderId, @Size, @Gender, @ImageUrl)
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

            var createdEntity = await GetByName(entity.ModelName);

            if(createdEntity == null)
                throw new Exception("T-Shirt not found!");

            return createdEntity;
        }

        public async Task Update(long id, TShirtModel entity)
        {
            var currentModel = await GetById(id);
            currentModel.Price = entity.Price;
            currentModel.ModelDescription = entity.ModelDescription;
            currentModel.IsActive = entity.IsActive;
            currentModel.ModelName = entity.ModelName;
            currentModel.QuantityInStock = entity.QuantityInStock;
            currentModel.Color = entity.Color;
            currentModel.ProviderId = entity.ProviderId;
            currentModel.Size = entity.Size;
            currentModel.Gender = entity.Gender;

            var query = $@"UPDATE BS_TSHIRTS
                            SET 
                                NAME = @ModelName, 
                                DESCRIPTION = @ModelDescription, 
                                PRICE = @Price, 
                                QUANTITY_IN_STOCK = @QuantityInStock, 
                                CREATED_ON = @CreatedOn, 
                                IS_ACTIVE = @IsActive,
                                COLOR = @Color,
                                PROVIDER_ID = @ProviderId,
                                SIZE = @Size,
                                GENDER = @Gender,
                                IMAGE_URL = @ImageUrl
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

        public async Task UpdateImage(long id, string imageUrl)
        {
            var idExists = await GetById(id);

            var query = $@"UPDATE BS_TSHIRTS
                            SET IMAGE_URL = @imageUrl
                            WHERE ID = @id
                            ";
            try
            {
                _uow.BeginTransaction();
                await _session.Connection.ExecuteScalarAsync(query, new {id, imageUrl}, _session.Transaction);
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
            var query = "DELETE FROM BS_TSHIRTS WHERE ID = @id";

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
        private async Task<TShirtModel> GetByName(string name, bool onlyActives = true)
        {

            var query = $@"
                        SELECT
                            BSTS.ID                      AS Id,
                            BSTS.NAME                    AS ModelName,
                            BSTS.DESCRIPTION             AS ModelDescription,
                            BSTS.PRICE                   AS Price,
                            BSTS.QUANTITY_IN_STOCK       AS QuantityInStock,
                            BSTS.CREATED_ON              AS CreatedOn,
                            BSTS.IS_ACTIVE               AS IsActive,
                            BSTS.SIZE                  AS Size,
                            BSTS.PROVIDER_ID             AS ProviderId,
                            BSTS.COLOR                   AS Color,
                            BSTS.GENDER                  AS Gender,
                            BSP.NAME                     AS Brand,
                            BSTS.IMAGE_URL               AS ImageUrl
                        FROM
                            BS_TSHIRTS BSTS
                        LEFT JOIN 
                            BS_PROVIDERS BSP 
                                ON 
                            BSTS.PROVIDER_ID = BSP.ID
                        WHERE BSTS.NAME = @name
                        ";

            if (onlyActives)
                query += _onlyActivesQuery;

            return await _session.Connection.QueryFirstOrDefaultAsync<TShirtModel>(query, new { name }, _session.Transaction);
        }
    }
}
