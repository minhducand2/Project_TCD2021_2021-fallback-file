using ApiGen.Data.Entity;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public class D1100ShopCategoriesDataAccess : DbFactoryBase, ID1100ShopCategoriesDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D1100ShopCategoriesDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All ShopCategories Async
        /// </summary>
        /// <returns>IEnumerable<ShopCategories></returns>
        public async Task<IEnumerable<E1100ShopCategories>> GetAllAsync()
        {
            return await DbQueryAsync<E1100ShopCategories>("SELECT id,Name,Thumbnail FROM p1100ShopCategories");
        }

        /// <summary>
        /// Create ShopCategories Async
        /// </summary>
        /// <param name="shopCategories"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E1100ShopCategories shopCategories)
        {
            string sqlQuery = $@"INSERT INTO p1100ShopCategories(Name,Thumbnail)
                                OUTPUT INSERTED.ID
                                 VALUES(@Name,@Thumbnail); ";

            return await DbQuerySingleAsync<long>(sqlQuery, shopCategories);
        }

        /// <summary>
        /// Update ShopCategories Async
        /// </summary>
        /// <param name="shopCategories"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E1100ShopCategories shopCategories)
        {
            string sqlQuery = $@"UPDATE p1100ShopCategories SET Name=@Name, Thumbnail=@Thumbnail
                                 WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, shopCategories);
        }

        /// <summary>
        /// Delete ShopCategories Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p1100ShopCategories
                                WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E1100ShopCategories> GetByIdAsync(object id)
        {
            string sqlQuery = $@"SELECT * FROM p1100ShopCategories
                                                    WHERE id = @id";
            return await DbQuerySingleAsync<E1100ShopCategories>(sqlQuery, new { id });
        }

        /// <summary>
        /// Get ShopCategoriess Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E1100ShopCategories>> GetPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E1100ShopCategories> shopCategoriess;
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM p1100ShopCategories   " + urlQueryParameters.condition + "   ");
            query.Append("  ORDER BY id OFFSET " + urlQueryParameters.offset + " ROWS FETCH NEXT " + urlQueryParameters.limit + " ROWS ONLY");
             

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            shopCategoriess = await DbQueryAsync<E1100ShopCategories>(query.ToString(), parameters);

            return shopCategoriess;
        } 

        /// <summary>
        /// Count ShopCategories item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p1100ShopCategories " + condition;
            return await DbQueryAsync<object>(sqlQuery, new { condition });
        }

        /// <summary>
        /// Execute With Transaction Scope
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ExecuteWithTransactionScope()
        {

            using (var dbCon = new SqlConnection(DbConnectionString))
            {
                await dbCon.OpenAsync();
                var transaction = await dbCon.BeginTransactionAsync();

                try
                {
                    // Do stuff here Insert, Update or Delete
                    Task q1 = dbCon.ExecuteAsync("<Your SQL Query here>");
                    Task q2 = dbCon.ExecuteAsync("<Your SQL Query here>");
                    Task q3 = dbCon.ExecuteAsync("<Your SQL Query here>");

                    await Task.WhenAll(q1, q2, q3);

                    //Commit the Transaction when all query are executed successfully

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    // Rollback the Transaction when any query fails
                    transaction.Rollback();
                    _logger.Log(LogLevel.Error, ex, "Error when trying to execute database operations within a scope.");

                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Get All ShopCategories Join ShopCategories
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CustomJoin()
        {
            return await DbQueryAsync<object>("SELECT p1100ShopCategories.*, Person.* FROM p1100ShopCategories INNER JOIN Person on p1100ShopCategories.id = Person.Id");
        }

        public async Task<IEnumerable<E1100ShopCategories>> GetAllRamdomLimit16Async()
        {
            return await DbQueryAsync<E1100ShopCategories>("SELECT * FROM p1100ShopCategories ORDER BY NEWID() OFFSET 0 ROWS FETCH NEXT 16 ROWS ONLY");
        }
    }
}