using B_Skin_Api.Data.Dapper;
using B_Skin_Api.Domain.Enums;
using B_Skin_Api.Domain.Interfaces;
using B_Skin_Api.Domain.Models;
using B_Skin_Api.Domain.Models.Dtos;
using B_Skin_Api.Domain.Models.Queries.TShirtQueries;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<TShirtDTO>> GetAll(GetAllTShirtsQuery query)
        {
            string orderBy = "";
            string querySql = $@"
                                SELECT
                                    BSTS.ID                      AS Id,
                                    BSTS.NAME                    AS ModelName,
                                    BSTS.DESCRIPTION             AS ModelDescription,
                                    BSTS.PRICE                   AS Price,
                                    BSTS.QUANTITY_IN_STOCK       AS QuantityInStock,
                                    BSTS.CREATED_ON              AS CreatedOn,
                                    BSTS.IS_ACTIVE               AS IsActive,
                                    BSTS.PROVIDER_ID             AS ProviderId,
                                    BSTS.COLOR                   AS Color,
                                    BSTS.GENDER                  AS Gender,
                                    BSP.NAME                     AS Brand,
                                    BSTS.IMAGE_URL               AS ImageUrl,
                                    BSTS.PROVIDER_ID             AS ProviderId
                                FROM
                                    BS_TSHIRTS BSTS
                                LEFT JOIN
                                    BS_PROVIDERS BSP
                                        ON 
                                    BSTS.PROVIDER_ID = BSP.ID
                                WHERE 1 = 1
                              ";

            if (query.OnlyActives)
                querySql += _onlyActivesQuery;

            if (query.Filters != null)
            {
                if (query.Filters.InitialPrice != null && query.Filters.FinalPrice != null)
                    querySql += $@" AND BSTS.PRICE BETWEEN {query.Filters.InitialPrice} and {query.Filters.FinalPrice}";

                if (query.Filters.InitialPrice != null && query.Filters.FinalPrice == null)
                    querySql += $@" AND BSTS.PRICE >= {query.Filters.InitialPrice}";

                if (query.Filters.InitialPrice == null && query.Filters.FinalPrice != null)
                    querySql += $@" AND BSTS.PRICE <= {query.Filters.FinalPrice}";

                if (query.Filters.ProviderId != null)
                    querySql += $@" AND BSTS.PROVIDER_ID = {query.Filters.ProviderId}";

                if (!string.IsNullOrEmpty(query.Filters.Gender))
                    querySql += $@" AND BSTS.GENDER = '{query.Filters.Gender.ToUpper()}'";

                if (query.Filters.OrderBy != null)
                {
                    if (query.Filters.OrderBy == EOrderBy.HighPrice)
                        orderBy = " ORDER BY PRICE DESC";

                    if (query.Filters.OrderBy == EOrderBy.LowPrice)
                        orderBy = " ORDER BY PRICE";
                }
            }

            if (string.IsNullOrEmpty(orderBy))
                orderBy = "  ORDER BY BSTS.NAME";

            querySql += orderBy;

            if (query.Pagination != null)
            {
                if (!query.Pagination.IgnorePagination)
                {
                    var paginationFilter = new PaginationFilter(query.Pagination.Page - 1, query.Pagination.PageSize);
                    querySql += $@" OFFSET ({paginationFilter.Page * paginationFilter.PageSize}) ROWS FETCH FIRST {query.Pagination.PageSize} ROWS ONLY";
                }
            }

            var result = await _session.Connection.QueryAsync<TShirtDTO>(querySql, null, _session.Transaction);

            if (result != null)
            {
                foreach (var item in result)
                {
                    var avaiableSizes = await GetAvaiableSizes(item.Id);
                    item.AvaiableSizes = avaiableSizes;
                }
            }

            if (query.Filters != null)
            {
                if (query.Filters.Size != null)
                {
                    string size = "";
                    switch (query.Filters.Size)
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

                    return result.Where(x => x.AvaiableSizes.Any(x => x.Size.Equals(size)));
                }
            }

            return result;
        }

        public async Task<IEnumerable<TShirtDTO>> SearchTShirtsByKeyWords(string querySearch, int resultsLimit)
        {
            string query = $@"
                        SELECT TOP {resultsLimit}
                            BSTS.ID                      AS Id,
                            BSTS.NAME                    AS ModelName,
                            BSTS.DESCRIPTION             AS ModelDescription,
                            BSTS.PRICE                   AS Price,
                            BSTS.QUANTITY_IN_STOCK       AS QuantityInStock,
                            BSTS.CREATED_ON              AS CreatedOn,
                            BSTS.IS_ACTIVE               AS IsActive,
                            BSTS.PROVIDER_ID             AS ProviderId,
                            BSTS.COLOR                   AS Color,
                            BSTS.GENDER                  AS Gender,
                            BSP.NAME                     AS Brand,
                            BSTS.IMAGE_URL               AS ImageUrl,
                            BSTS.PROVIDER_ID             AS ProviderId
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

            var result = await _session.Connection.QueryAsync<TShirtDTO>(query, null, _session.Transaction);

            if (result != null)
            {
                foreach (var item in result)
                {
                    var avaiableSizes = await GetAvaiableSizes(item.Id);
                    item.AvaiableSizes = avaiableSizes;
                }
            }

            return result;
        }

        public async Task<TShirtDTO> GetById(long id, bool onlyActives = true)
        {
            var result = await GetAll(new GetAllTShirtsQuery
            {
                Filters = null,
                OnlyActives = onlyActives,
                Pagination = null
            });

            if (result == null)
                throw new Exception("T-Shirt not found!");

            return result.FirstOrDefault(x => x.Id == id);
        }

        public async Task InactivateById(long id)
        {
            string query = $@"
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
            string query = $@"
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

        public async Task<TShirtDTO> Create(TShirtModel entity, string sizeIds)
        {
            var separatedSizes = SeparateSizes(sizeIds);
            TShirtDTO createdEntity = null;

            if (entity.Gender != null)
                entity.Gender = entity.Gender.ToUpper();

            string queryTShirt = $@"INSERT INTO BS_TSHIRTS
                                    (NAME, DESCRIPTION, PRICE, QUANTITY_IN_STOCK, CREATED_ON, IS_ACTIVE, IMAGE_URL, PROVIDER_ID, COLOR, GENDER)
                                    VALUES
                                    (@ModelName, @ModelDescription, @Price, @QuantityInStock, @CreatedOn, @IsActive, @ImageUrl, @ProviderId, @Color, @Gender)
                                   ";

            string querySizes = $@"INSERT INTO BS_TSHIRTS_SIZE
                                    (SIZE_ID, TSHIRT_ID)
                                    VALUES(@size, @id)
                                  ";
            try
            {
                _uow.BeginTransaction();
                await _session.Connection.ExecuteScalarAsync(queryTShirt, entity, _session.Transaction);
                
                createdEntity = await GetCreatedRegister(entity);

                foreach (int size in separatedSizes)
                {
                    long id = createdEntity.Id;
                    await _session.Connection.ExecuteScalarAsync(querySizes, new { size, id }, _session.Transaction);
                }

                createdEntity.AvaiableSizes = await GetAvaiableSizes(createdEntity.Id);

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

            if (createdEntity == null)
                throw new Exception("An error ocurred during the process.");

            return createdEntity;
        }

        public async Task Update(TShirtModel entity, string sizeIds)
        {
            long id = entity.Id;
            var separatedSizes = SeparateSizes(sizeIds);

            var currentModel = await GetById(entity.Id);
            if (currentModel == null) throw new Exception("T-Shirt not found.");

            if (entity.Gender != null)
                entity.Gender = entity.Gender.ToUpper();

            string querySizes = $@"INSERT INTO BS_TSHIRTS_SIZE (SIZE_ID, TSHIRT_ID) VALUES(@size, @id)";

            string queryDeletePrevious = $@"DELETE FROM BS_TSHIRTS_SIZE WHERE TSHIRT_ID = @id";

            string queryTShirt = $@"UPDATE BS_TSHIRTS SET 
                                    NAME = @ModelName, 
                                    DESCRIPTION = @ModelDescription, 
                                    PRICE = @Price, 
                                    QUANTITY_IN_STOCK = @QuantityInStock, 
                                    IS_ACTIVE = @IsActive,
                                    IMAGE_URL = @ImageUrl,
                                    PROVIDER_ID = @ProviderId,
                                    COLOR = @Color,
                                    GENDER = @Gender
                                    WHERE ID = @Id
                                 ";

            try
            {
                _uow.BeginTransaction();
                await _session.Connection.ExecuteScalarAsync(queryTShirt, entity, _session.Transaction);
                await _session.Connection.ExecuteScalarAsync(queryDeletePrevious, new {id}, _session.Transaction);

                foreach (int size in separatedSizes)
                {
                    await _session.Connection.ExecuteScalarAsync(querySizes, new { size, id }, _session.Transaction);
                }

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

            string query = $@"UPDATE BS_TSHIRTS
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
            string query = "DELETE FROM BS_TSHIRTS WHERE ID = @id";

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


        public async Task<IEnumerable<SizeModel>> GetAvaiableSizes(long tshirtId)
        {
            string querySql = $@"
                                SELECT DISTINCT [SIZE] FROM BS_SIZE BSS
                                LEFT JOIN BS_TSHIRTS_SIZE BTS
                                ON BSS.ID = BTS.SIZE_ID
                                WHERE BTS.TSHIRT_ID = @tshirtId
                                ";

            return await _session.Connection.QueryAsync<SizeModel>(querySql, new { tshirtId }, _session.Transaction);
        }
        private async Task<TShirtDTO> GetCreatedRegister(TShirtModel model)
        {

            var result = await GetAll(new GetAllTShirtsQuery
            {
                Filters = null,
                OnlyActives = true,
                Pagination = null
            });

            return result.FirstOrDefault(x => x.ModelName == model.ModelName && x.ModelDescription == model.ModelDescription && x.Color == model.Color);
        }

        private IEnumerable<int> SeparateSizes(string sizes)
        {
            var separate = sizes.Trim().Split(',');
            var separatedSizes = new List<int>();
            foreach(var size in separate) 
            {
                separatedSizes.Add(int.Parse(size));
            }

            if (!separatedSizes.Any())
                throw new Exception("Size(s) not found.");

            return separatedSizes;
        }
    }
}
