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
    public class D2200OrderStatusDataAccess : DbFactoryBase, ID2200OrderStatusDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D2200OrderStatusDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All OrderStatus Async
        /// </summary>
        /// <returns>IEnumerable<OrderStatus></returns>
        public async Task<IEnumerable<E2200OrderStatus>> GetAllAsync()
        {
            return await DbQueryAsync<E2200OrderStatus>("SELECT id,Name FROM p2200OrderStatus");
        }

        /// <summary>
        /// Create OrderStatus Async
        /// </summary>
        /// <param name="orderStatus"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E2200OrderStatus orderStatus)
        {
            string sqlQuery = $@"INSERT INTO p2200OrderStatus(Name)
                                 OUTPUT INSERTED.ID
                                 VALUES(@Name);
                                  ";

            return await DbQuerySingleAsync<long>(sqlQuery, orderStatus);
        }

        /// <summary>
        /// Update OrderStatus Async
        /// </summary>
        /// <param name="orderStatus"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E2200OrderStatus orderStatus)
        {
            string sqlQuery = $@"UPDATE p2200OrderStatus SET Name=@Name
                                 WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, orderStatus);
        }

        /// <summary>
        /// Delete OrderStatus Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p2200OrderStatus
                                WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E2200OrderStatus> GetByIdAsync(object id)
        {
            string sqlQuery = $@"SELECT * FROM p2200OrderStatus
                                                    WHERE id = @id";
            return await DbQuerySingleAsync<E2200OrderStatus>(sqlQuery, new { id });
        }

        /// <summary>
        /// Get OrderStatuss Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E2200OrderStatus>> GetPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E2200OrderStatus> orderStatuss;
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM p2200OrderStatus   " + urlQueryParameters.condition + "   ");
            query.Append("  ORDER BY id OFFSET " + urlQueryParameters.offset + " ROWS FETCH NEXT " + urlQueryParameters.limit + " ROWS ONLY");

 

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            orderStatuss = await DbQueryAsync<E2200OrderStatus>(query.ToString(), parameters);

            return orderStatuss;
        } 

        /// <summary>
        /// Count OrderStatus item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p2200OrderStatus " + condition;
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
        /// Get All OrderStatus Join OrderStatus
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CustomJoin()
        {
            return await DbQueryAsync<object>("SELECT p2200OrderStatus.*, Person.* FROM p2200OrderStatus INNER JOIN Person on p2200OrderStatus.id = Person.Id");
        }
    }
}