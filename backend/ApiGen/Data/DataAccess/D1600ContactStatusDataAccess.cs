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
    public class D1600ContactStatusDataAccess : DbFactoryBase, ID1600ContactStatusDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D1600ContactStatusDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All ContactStatus Async
        /// </summary>
        /// <returns>IEnumerable<ContactStatus></returns>
        public async Task<IEnumerable<E1600ContactStatus>> GetAllAsync()
        {
            return await DbQueryAsync<E1600ContactStatus>("SELECT id,Name FROM p1600ContactStatus");
        }

        /// <summary>
        /// Create ContactStatus Async
        /// </summary>
        /// <param name="contactStatus"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E1600ContactStatus contactStatus)
        {
            string sqlQuery = $@"INSERT INTO p1600ContactStatus(Name)
                                 OUTPUT INSERTED.ID
                                 VALUES(@Name); ";

            return await DbQuerySingleAsync<long>(sqlQuery, contactStatus);
        }

        /// <summary>
        /// Update ContactStatus Async
        /// </summary>
        /// <param name="contactStatus"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E1600ContactStatus contactStatus)
        {
            string sqlQuery = $@"UPDATE p1600ContactStatus SET Name=@Name
                                 WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, contactStatus);
        }

        /// <summary>
        /// Delete ContactStatus Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p1600ContactStatus
                                WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E1600ContactStatus> GetByIdAsync(object id)
        {
            string sqlQuery = $@"SELECT * FROM p1600ContactStatus
                                                    WHERE id = @id";
            return await DbQuerySingleAsync<E1600ContactStatus>(sqlQuery, new { id });
        }

        /// <summary>
        /// Get ContactStatuss Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E1600ContactStatus>> GetPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E1600ContactStatus> contactStatuss;
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM p1600ContactStatus   " + urlQueryParameters.condition + "   ");
            query.Append("  ORDER BY id OFFSET " + urlQueryParameters.offset + " ROWS FETCH NEXT " + urlQueryParameters.limit + " ROWS ONLY");
             

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            contactStatuss = await DbQueryAsync<E1600ContactStatus>(query.ToString(), parameters);

            return contactStatuss;
        } 

        /// <summary>
        /// Count ContactStatus item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p1600ContactStatus " + condition;
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
        /// Get All ContactStatus Join ContactStatus
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CustomJoin()
        {
            return await DbQueryAsync<object>("SELECT p1600ContactStatus.*, Person.* FROM p1600ContactStatus INNER JOIN Person on p1600ContactStatus.id = Person.Id");
        }
    }
}