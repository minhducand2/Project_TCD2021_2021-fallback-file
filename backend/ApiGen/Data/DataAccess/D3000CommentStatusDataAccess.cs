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
    public class D3000CommentStatusDataAccess : DbFactoryBase, ID3000CommentStatusDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D3000CommentStatusDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All CommentStatus Async
        /// </summary>
        /// <returns>IEnumerable<CommentStatus></returns>
        public async Task<IEnumerable<E3000CommentStatus>> GetAllAsync()
        {
            return await DbQueryAsync<E3000CommentStatus>("SELECT id,Name FROM p3000CommentStatus");
        }

        /// <summary>
        /// Create CommentStatus Async
        /// </summary>
        /// <param name="commentStatus"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E3000CommentStatus commentStatus)
        {
            string sqlQuery = $@"INSERT INTO p3000CommentStatus(Name)
                                 OUTPUT INSERTED.ID
                                 VALUES(@Name);
                                  ";

            return await DbQuerySingleAsync<long>(sqlQuery, commentStatus);
        }

        /// <summary>
        /// Update CommentStatus Async
        /// </summary>
        /// <param name="commentStatus"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E3000CommentStatus commentStatus)
        {
            string sqlQuery = $@"UPDATE p3000CommentStatus SET Name=@Name
                                 WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, commentStatus);
        }

        /// <summary>
        /// Delete CommentStatus Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p3000CommentStatus
                                WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E3000CommentStatus> GetByIdAsync(object id)
        {
            string sqlQuery = $@"SELECT * FROM p3000CommentStatus
                                                    WHERE id = @id";
            return await DbQuerySingleAsync<E3000CommentStatus>(sqlQuery, new { id });
        }

        /// <summary>
        /// Get CommentStatuss Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E3000CommentStatus>> GetPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E3000CommentStatus> commentStatuss;
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM p3000CommentStatus   " + urlQueryParameters.condition + "   ");
            query.Append("  ORDER BY id OFFSET " + urlQueryParameters.offset + " ROWS FETCH NEXT " + urlQueryParameters.limit + " ROWS ONLY");
             

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            commentStatuss = await DbQueryAsync<E3000CommentStatus>(query.ToString(), parameters);

            return commentStatuss;
        } 

        /// <summary>
        /// Count CommentStatus item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p3000CommentStatus " + condition;
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
        /// Get All CommentStatus Join CommentStatus
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CustomJoin()
        {
            return await DbQueryAsync<object>("SELECT p3000CommentStatus.*, Person.* FROM p3000CommentStatus INNER JOIN Person on p3000CommentStatus.id = Person.Id");
        }
    }
}