using ApiGen.Data.Entity;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public class D1300BlogCategoriesDataAccess : DbFactoryBase, ID1300BlogCategoriesDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D1300BlogCategoriesDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All BlogCategories Async
        /// </summary>
        /// <returns>IEnumerable<BlogCategories></returns>
        public async Task<IEnumerable<E1300BlogCategories>> GetAllAsync()
        {
            return await DbQueryAsync<E1300BlogCategories>("SELECT id,Name FROM p1300BlogCategories");
        }

        /// <summary>
        /// Create BlogCategories Async
        /// </summary>
        /// <param name="blogCategories"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E1300BlogCategories blogCategories)
        {
            string sqlQuery = $@"INSERT INTO p1300BlogCategories(Name) 
                                OUTPUT INSERTED.ID
                                 VALUES(@Name); ";

            return await DbQuerySingleAsync<long>(sqlQuery, blogCategories);
        }

        /// <summary>
        /// Update BlogCategories Async
        /// </summary>
        /// <param name="blogCategories"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E1300BlogCategories blogCategories)
        {
            string sqlQuery = $@"UPDATE p1300BlogCategories SET Name=@Name
                                 WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, blogCategories);
        }

        /// <summary>
        /// Delete BlogCategories Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p1300BlogCategories
                                WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E1300BlogCategories> GetByIdAsync(object id)
        {
            string sqlQuery = $@"SELECT * FROM p1300BlogCategories
                                                    WHERE id = @id";
            return await DbQuerySingleAsync<E1300BlogCategories>(sqlQuery, new { id });
        }

        /// <summary>
        /// Get BlogCategoriess Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E1300BlogCategories>> GetPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E1300BlogCategories> blogCategoriess;

            var query = @"SELECT *                                                                              
                            FROM (SELECT id FROM p1300BlogCategories ORDER BY id OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY) T1     
                            INNER JOIN p1300BlogCategories T2 ON T1.id = T2.id                                               
                                " + urlQueryParameters.condition;

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            blogCategoriess = await DbQueryAsync<E1300BlogCategories>(query, parameters);

            return blogCategoriess;
        } 

        /// <summary>
        /// Count BlogCategories item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p1300BlogCategories " + condition;
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
        /// Get All BlogCategories Join BlogCategories
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CustomJoin()
        {
            return await DbQueryAsync<object>("SELECT p1300BlogCategories.*, Person.* FROM p1300BlogCategories INNER JOIN Person on p1300BlogCategories.id = Person.Id");
        }
    }
}