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
    public class D1500ContactInfoDataAccess : DbFactoryBase, ID1500ContactInfoDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D1500ContactInfoDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All ContactInfo Async
        /// </summary>
        /// <returns>IEnumerable<ContactInfo></returns>
        public async Task<IEnumerable<E1500ContactInfo>> GetAllAsync()
        {
            return await DbQueryAsync<E1500ContactInfo>("SELECT id,Address,Phone,Mail,Working,Facebook,Instagram,Youtube,Twitter,Map FROM p1500ContactInfo");
        }

        /// <summary>
        /// Create ContactInfo Async
        /// </summary>
        /// <param name="contactInfo"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E1500ContactInfo contactInfo)
        {
            string sqlQuery = $@"INSERT INTO p1500ContactInfo(Address,Phone,Mail,Working,Facebook,Instagram,Youtube,Twitter,Map)
                                 OUTPUT INSERTED.ID
                                 VALUES(@Address,@Phone,@Mail,@Working,@Facebook,@Instagram,@Youtube,@Twitter,@Map);
                                 ";

            return await DbQuerySingleAsync<long>(sqlQuery, contactInfo);
        }

        /// <summary>
        /// Update ContactInfo Async
        /// </summary>
        /// <param name="contactInfo"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E1500ContactInfo contactInfo)
        {
            string sqlQuery = $@"UPDATE p1500ContactInfo SET Address=@Address,Phone=@Phone,Mail=@Mail,Working=@Working,Facebook=@Facebook,Instagram=@Instagram,Youtube=@Youtube,Twitter=@Twitter,Map=@Map
                                 WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, contactInfo);
        }

        /// <summary>
        /// Delete ContactInfo Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p1500ContactInfo
                                WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E1500ContactInfo> GetByIdAsync(object id)
        {
            string sqlQuery = $@"SELECT * FROM p1500ContactInfo
                                                    WHERE id = @id";
            return await DbQuerySingleAsync<E1500ContactInfo>(sqlQuery, new { id });
        }

        /// <summary>
        /// Get ContactInfos Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E1500ContactInfo>> GetPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E1500ContactInfo> contactInfos;
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM p1500ContactInfo   " + urlQueryParameters.condition + "   ");
            query.Append("  ORDER BY id OFFSET " + urlQueryParameters.offset + " ROWS FETCH NEXT " + urlQueryParameters.limit + " ROWS ONLY");

             

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            contactInfos = await DbQueryAsync<E1500ContactInfo>(query.ToString(), parameters);

            return contactInfos;
        } 

        /// <summary>
        /// Count ContactInfo item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p1500ContactInfo " + condition;
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
        /// Get All ContactInfo Join ContactInfo
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CustomJoin()
        {
            return await DbQueryAsync<object>("SELECT p1500ContactInfo.*, Person.* FROM p1500ContactInfo INNER JOIN Person on p1500ContactInfo.id = Person.Id");
        }
    }
}