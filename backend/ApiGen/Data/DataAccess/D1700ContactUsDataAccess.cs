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
    public class D1700ContactUsDataAccess : DbFactoryBase, ID1700ContactUsDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D1700ContactUsDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All ContactUs Async
        /// </summary>
        /// <returns>IEnumerable<ContactUs></returns>
        public async Task<IEnumerable<E1700ContactUs>> GetAllAsync()
        {
            return await DbQueryAsync<E1700ContactUs>("SELECT id,IdContactStatus,Name,Email,Message FROM p1700ContactUs");
        }

        /// <summary>
        /// Create ContactUs Async
        /// </summary>
        /// <param name="contactUs"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E1700ContactUs contactUs)
        {
            string sqlQuery = $@"INSERT INTO p1700ContactUs(IdContactStatus,Name,Email,Message)
                                    OUTPUT INSERTED.ID
                                 VALUES(@IdContactStatus,@Name,@Email,@Message); ";

            return await DbQuerySingleAsync<long>(sqlQuery, contactUs);
        }

        /// <summary>
        /// Update ContactUs Async
        /// </summary>
        /// <param name="contactUs"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E1700ContactUs contactUs)
        {
            string sqlQuery = $@"UPDATE p1700ContactUs SET IdContactStatus=@IdContactStatus,Name=@Name,Email=@Email,Message=@Message
                                 WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, contactUs);
        }

        /// <summary>
        /// Delete ContactUs Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p1700ContactUs
                                WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E1700ContactUs> GetByIdAsync(object id)
        {
            string sqlQuery = $@"SELECT * FROM p1700ContactUs
                                                    WHERE id = @id";
            return await DbQuerySingleAsync<E1700ContactUs>(sqlQuery, new { id });
        }

        /// <summary>
        /// Get ContactUss Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E1700ContactUs>> GetPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E1700ContactUs> contactUss;
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM p1700ContactUs   " + urlQueryParameters.condition + "   ");
            query.Append("  ORDER BY id OFFSET " + urlQueryParameters.offset + " ROWS FETCH NEXT " + urlQueryParameters.limit + " ROWS ONLY");
 

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            contactUss = await DbQueryAsync<E1700ContactUs>(query.ToString(), parameters);

            return contactUss;
        } 

        /// <summary>
        /// Count ContactUs item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p1700ContactUs " + condition;
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
        /// Get All ContactUs Join ContactUs
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CustomJoin()
        {
            return await DbQueryAsync<object>("SELECT p1700ContactUs.*, Person.* FROM p1700ContactUs INNER JOIN Person on p1700ContactUs.id = Person.Id");
        }
    }
}