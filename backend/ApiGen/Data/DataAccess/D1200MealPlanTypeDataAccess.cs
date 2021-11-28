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
    public class D1200MealPlanTypeDataAccess : DbFactoryBase, ID1200MealPlanTypeDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D1200MealPlanTypeDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All MealPlanType Async
        /// </summary>
        /// <returns>IEnumerable<MealPlanType></returns>
        public async Task<IEnumerable<E1200MealPlanType>> GetAllAsync()
        {
            return await DbQueryAsync<E1200MealPlanType>("SELECT id,Name FROM p1200MealPlanType");
        }

        /// <summary>
        /// Create MealPlanType Async
        /// </summary>
        /// <param name="mealPlanType"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E1200MealPlanType mealPlanType)
        {
            string sqlQuery = $@"INSERT INTO p1200MealPlanType(Name)
                                OUTPUT INSERTED.ID
                                 VALUES(@Name); ";

            return await DbQuerySingleAsync<long>(sqlQuery, mealPlanType);
        }

        /// <summary>
        /// Update MealPlanType Async
        /// </summary>
        /// <param name="mealPlanType"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E1200MealPlanType mealPlanType)
        {
            string sqlQuery = $@"UPDATE p1200MealPlanType SET Name=@Name
                                 WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, mealPlanType);
        }

        /// <summary>
        /// Delete MealPlanType Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p1200MealPlanType
                                WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E1200MealPlanType> GetByIdAsync(object id)
        {
            string sqlQuery = $@"SELECT * FROM p1200MealPlanType
                                                    WHERE id = @id";
            return await DbQuerySingleAsync<E1200MealPlanType>(sqlQuery, new { id });
        }

        /// <summary>
        /// Get MealPlanTypes Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E1200MealPlanType>> GetPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E1200MealPlanType> mealPlanTypes;

            var query = @"SELECT *                                                                              
                            FROM (SELECT id FROM p1200MealPlanType ORDER BY id OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY) T1     
                            INNER JOIN p1200MealPlanType T2 ON T1.id = T2.id                                               
                                " + urlQueryParameters.condition;

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            mealPlanTypes = await DbQueryAsync<E1200MealPlanType>(query, parameters);

            return mealPlanTypes;
        } 

        /// <summary>
        /// Count MealPlanType item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p1200MealPlanType " + condition;
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
        /// Get All MealPlanType Join MealPlanType
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CustomJoin()
        {
            return await DbQueryAsync<object>("SELECT p1200MealPlanType.*, Person.* FROM p1200MealPlanType INNER JOIN Person on p1200MealPlanType.id = Person.Id");
        }
    }
}