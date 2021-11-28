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
    public class D500FooterDataAccess : DbFactoryBase, ID500FooterDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D500FooterDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All Footer Async
        /// </summary>
        /// <returns>IEnumerable<Footer></returns>
        public async Task<IEnumerable<E500Footer>> GetAllAsync()
        {
            return await DbQueryAsync<E500Footer>("SELECT id,Content1,Content2,Content3,AndroidLink,IosLink FROM p500Footer");
        }

        /// <summary>
        /// Create Footer Async
        /// </summary>
        /// <param name="footer"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E500Footer footer)
        {
            string sqlQuery = $@"INSERT INTO p500Footer(Content1,Content2,Content3,AndroidLink,IosLink)
                                    OUTPUT INSERTED.ID
                                 VALUES(@Content1,@Content2,@Content3,@AndroidLink,@IosLink); ";

            return await DbQuerySingleAsync<long>(sqlQuery, footer);
        }

        /// <summary>
        /// Update Footer Async
        /// </summary>
        /// <param name="footer"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E500Footer footer)
        {
            string sqlQuery = $@"UPDATE p500Footer SET Content1=@Content1,Content2=@Content2,Content3=@Content3,AndroidLink=@AndroidLink,IosLink=@IosLink
                                 WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, footer);
        }

        /// <summary>
        /// Delete Footer Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p500Footer
                                WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E500Footer> GetByIdAsync(object id)
        {
            string sqlQuery = $@"SELECT * FROM p500Footer
                                                    WHERE id = @id";
            return await DbQuerySingleAsync<E500Footer>(sqlQuery, new { id });
        }

        /// <summary>
        /// Get Footers Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E500Footer>> GetPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E500Footer> footers;
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM p500Footer   " + urlQueryParameters.condition + "   ");
            query.Append("  ORDER BY id OFFSET " + urlQueryParameters.offset + " ROWS FETCH NEXT " + urlQueryParameters.limit + " ROWS ONLY");

             

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            footers = await DbQueryAsync<E500Footer>(query.ToString(), parameters);

            return footers;
        } 

        /// <summary>
        /// Count Footer item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p500Footer " + condition;
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
        /// Get All Footer Join Footer
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CustomJoin()
        {
            return await DbQueryAsync<object>("SELECT p500Footer.*, Person.* FROM p500Footer INNER JOIN Person on p500Footer.id = Person.Id");
        }
    }
}