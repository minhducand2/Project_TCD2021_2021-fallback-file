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
    public class D1900RoleUserDataAccess : DbFactoryBase, ID1900RoleUserDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D1900RoleUserDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All RoleUser Async
        /// </summary>
        /// <returns>IEnumerable<RoleUser></returns>
        public async Task<IEnumerable<E1900RoleUser>> GetAllAsync()
        {
            return await DbQueryAsync<E1900RoleUser>("SELECT id,Name FROM p1900RoleUser");
        }

        /// <summary>
        /// Create RoleUser Async
        /// </summary>
        /// <param name="roleUser"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E1900RoleUser roleUser)
        {
            string sqlQuery = $@"INSERT INTO p1900RoleUser(Name)
                                 OUTPUT INSERTED.ID
                                 VALUES(@Name); ";

            return await DbQuerySingleAsync<long>(sqlQuery, roleUser);
        }

        /// <summary>
        /// Update RoleUser Async
        /// </summary>
        /// <param name="roleUser"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E1900RoleUser roleUser)
        {
            string sqlQuery = $@"UPDATE p1900RoleUser SET Name=@Name
                                 WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, roleUser);
        }

        /// <summary>
        /// Delete RoleUser Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p1900RoleUser
                                WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E1900RoleUser> GetByIdAsync(object id)
        {
            string sqlQuery = $@"SELECT * FROM p1900RoleUser
                                                    WHERE id = @id";
            return await DbQuerySingleAsync<E1900RoleUser>(sqlQuery, new { id });
        }

        /// <summary>
        /// Get RoleUsers Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E1900RoleUser>> GetPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E1900RoleUser> roleUsers;
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM p1900RoleUser   " + urlQueryParameters.condition + "   ");
            query.Append("  ORDER BY id OFFSET " + urlQueryParameters.offset + " ROWS FETCH NEXT " + urlQueryParameters.limit + " ROWS ONLY");
 

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            roleUsers = await DbQueryAsync<E1900RoleUser>(query.ToString(), parameters);

            return roleUsers;
        } 

        /// <summary>
        /// Count RoleUser item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p1900RoleUser " + condition;
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
        /// Get All RoleUser Join RoleUser
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CustomJoin()
        {
            return await DbQueryAsync<object>("SELECT p1900RoleUser.*, Person.* FROM p1900RoleUser INNER JOIN Person on p1900RoleUser.id = Person.Id");
        }
    }
}