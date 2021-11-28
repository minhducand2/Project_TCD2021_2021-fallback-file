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
    public class D3100MyPromotionDataAccess : DbFactoryBase, ID3100MyPromotionDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D3100MyPromotionDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All MyPromotion Async
        /// </summary>
        /// <returns>IEnumerable<MyPromotion></returns>
        public async Task<IEnumerable<E3100MyPromotion>> GetAllAsync()
        {
            return await DbQueryAsync<E3100MyPromotion>("SELECT id,IdPromotion,CreatedAt FROM p3100MyPromotion");
        }

        /// <summary>
        /// Create MyPromotion Async
        /// </summary>
        /// <param name="myPromotion"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E3100MyPromotion myPromotion)
        {
            string sqlQuery = $@"INSERT INTO p3100MyPromotion(IdPromotion,CreatedAt)
                                 OUTPUT INSERTED.ID
                                 VALUES(@IdPromotion,@CreatedAt);
                                 ";

            return await DbQuerySingleAsync<long>(sqlQuery, myPromotion);
        }

        /// <summary>
        /// Update MyPromotion Async
        /// </summary>
        /// <param name="myPromotion"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E3100MyPromotion myPromotion)
        {
            string sqlQuery = $@"UPDATE p3100MyPromotion SET IdPromotion=@IdPromotion,CreatedAt=@CreatedAt
                                 WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, myPromotion);
        }

        /// <summary>
        /// Delete MyPromotion Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p3100MyPromotion
                                WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E3100MyPromotion> GetByIdAsync(object id)
        {
            string sqlQuery = $@"SELECT * FROM p3100MyPromotion
                                                    WHERE id = @id";
            return await DbQuerySingleAsync<E3100MyPromotion>(sqlQuery, new { id });
        }

        /// <summary>
        /// Get MyPromotions Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E3100MyPromotion>> GetPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E3100MyPromotion> myPromotions;

            var query = @"SELECT *                                                                              
                            FROM (SELECT id FROM p3100MyPromotion ORDER BY id OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY) T1     
                            INNER JOIN p3100MyPromotion T2 ON T1.id = T2.id                                               
                                " + urlQueryParameters.condition;

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            myPromotions = await DbQueryAsync<E3100MyPromotion>(query, parameters);

            return myPromotions;
        } 

        /// <summary>
        /// Count MyPromotion item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p3100MyPromotion " + condition;
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
        /// Get All MyPromotion Join MyPromotion
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CustomJoin()
        {
            return await DbQueryAsync<object>("SELECT p3100MyPromotion.*, Person.* FROM p3100MyPromotion INNER JOIN Person on p3100MyPromotion.id = Person.Id");
        }
    }
}