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
    public class D1800UserStatusDataAccess : DbFactoryBase, ID1800UserStatusDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D1800UserStatusDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All UserStatus Async
        /// </summary>
        /// <returns>IEnumerable<UserStatus></returns>
        public async Task<IEnumerable<E1800UserStatus>> GetAllAsync()
        {
            return await DbQueryAsync<E1800UserStatus>("SELECT id,Name FROM p1800UserStatus");
        }

        /// <summary>
        /// Create UserStatus Async
        /// </summary>
        /// <param name="userStatus"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E1800UserStatus userStatus)
        {
            string sqlQuery = $@"INSERT INTO p1800UserStatus(Name)
                                    OUTPUT INSERTED.ID
                                 VALUES(@Name); ";

            return await DbQuerySingleAsync<long>(sqlQuery, userStatus);
        }

        /// <summary>
        /// Update UserStatus Async
        /// </summary>
        /// <param name="userStatus"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E1800UserStatus userStatus)
        {
            string sqlQuery = $@"UPDATE p1800UserStatus SET Name=@Name
                                 WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, userStatus);
        }

        /// <summary>
        /// Delete UserStatus Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p1800UserStatus
                                WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E1800UserStatus> GetByIdAsync(object id)
        {
            string sqlQuery = $@"SELECT * FROM p1800UserStatus
                                                    WHERE id = @id";
            return await DbQuerySingleAsync<E1800UserStatus>(sqlQuery, new { id });
        }

        /// <summary>
        /// Get UserStatuss Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E1800UserStatus>> GetPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E1800UserStatus> userStatuss;
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM p1800UserStatus   " + urlQueryParameters.condition + "   ");
            query.Append("  ORDER BY id OFFSET " + urlQueryParameters.offset + " ROWS FETCH NEXT " + urlQueryParameters.limit + " ROWS ONLY");
             

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            userStatuss = await DbQueryAsync<E1800UserStatus>(query.ToString(), parameters);

            return userStatuss;
        } 

        /// <summary>
        /// Count UserStatus item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p1800UserStatus " + condition;
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
        /// Get All UserStatus Join UserStatus
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CustomJoin()
        {
            return await DbQueryAsync<object>("SELECT p1800UserStatus.*, Person.* FROM p1800UserStatus INNER JOIN Person on p1800UserStatus.id = Person.Id");
        }
    }
}