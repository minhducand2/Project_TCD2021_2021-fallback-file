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
    public class D600HeaderInfoDataAccess : DbFactoryBase, ID600HeaderInfoDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D600HeaderInfoDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All HeaderInfo Async
        /// </summary>
        /// <returns>IEnumerable<HeaderInfo></returns>
        public async Task<IEnumerable<E600HeaderInfo>> GetAllAsync()
        {
            return await DbQueryAsync<E600HeaderInfo>("SELECT id,Phone,Mail,Logo FROM p600HeaderInfo");
        }

        /// <summary>
        /// Create HeaderInfo Async
        /// </summary>
        /// <param name="headerInfo"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E600HeaderInfo headerInfo)
        {
            string sqlQuery = $@"INSERT INTO p600HeaderInfo(Phone,Mail,Logo)
                                    OUTPUT INSERTED.ID
                                 VALUES(@Phone,@Mail,@Logo); ";

            return await DbQuerySingleAsync<long>(sqlQuery, headerInfo);
        }

        /// <summary>
        /// Update HeaderInfo Async
        /// </summary>
        /// <param name="headerInfo"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E600HeaderInfo headerInfo)
        {
            string sqlQuery = $@"UPDATE p600HeaderInfo SET Phone=@Phone,Mail=@Mail,Logo=@Logo
                                 WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, headerInfo);
        }

        /// <summary>
        /// Delete HeaderInfo Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p600HeaderInfo
                                WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E600HeaderInfo> GetByIdAsync(object id)
        {
            string sqlQuery = $@"SELECT * FROM p600HeaderInfo
                                                    WHERE id = @id";
            return await DbQuerySingleAsync<E600HeaderInfo>(sqlQuery, new { id });
        }

        /// <summary>
        /// Get HeaderInfos Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E600HeaderInfo>> GetPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E600HeaderInfo> headerInfos;
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM p600HeaderInfo   " + urlQueryParameters.condition + "   ");
            query.Append("  ORDER BY id OFFSET " + urlQueryParameters.offset + " ROWS FETCH NEXT " + urlQueryParameters.limit + " ROWS ONLY");
             

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            headerInfos = await DbQueryAsync<E600HeaderInfo>(query.ToString(), parameters);

            return headerInfos;
        } 

        /// <summary>
        /// Count HeaderInfo item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p600HeaderInfo " + condition;
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
        /// Get All HeaderInfo Join HeaderInfo
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CustomJoin()
        {
            return await DbQueryAsync<object>("SELECT p600HeaderInfo.*, Person.* FROM p600HeaderInfo INNER JOIN Person on p600HeaderInfo.id = Person.Id");
        }
    }
}