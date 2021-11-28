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
    public class D400BannerDataAccess : DbFactoryBase, ID400BannerDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D400BannerDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All Banner Async
        /// </summary>
        /// <returns>IEnumerable<Banner></returns>
        public async Task<IEnumerable<E400Banner>> GetAllAsync()
        {
            return await DbQueryAsync<E400Banner>("SELECT id,Image,Position FROM p400Banner");
        }

        /// <summary>
        /// Create Banner Async
        /// </summary>
        /// <param name="banner"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E400Banner banner)
        {
            string sqlQuery = $@"INSERT INTO p400Banner(Image,Position)
                                    OUTPUT INSERTED.ID
                                 VALUES(@Image,@Position); ";

            return await DbQuerySingleAsync<long>(sqlQuery, banner);
        }

        /// <summary>
        /// Update Banner Async
        /// </summary>
        /// <param name="banner"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E400Banner banner)
        {
            string sqlQuery = $@"UPDATE p400Banner SET Image=@Image,Position=@Position
                                 WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, banner);
        }

        /// <summary>
        /// Delete Banner Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p400Banner
                                WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E400Banner> GetByIdAsync(object id)
        {
            string sqlQuery = $@"SELECT * FROM p400Banner
                                                    WHERE id = @id";
            return await DbQuerySingleAsync<E400Banner>(sqlQuery, new { id });
        }

        /// <summary>
        /// Get Banners Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E400Banner>> GetPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E400Banner> banners;
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM p400Banner   " + urlQueryParameters.condition + "   ");
            query.Append("  ORDER BY id OFFSET " + urlQueryParameters.offset + " ROWS FETCH NEXT " + urlQueryParameters.limit + " ROWS ONLY");

 

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            banners = await DbQueryAsync<E400Banner>(query.ToString(), parameters);

            return banners;
        } 

        /// <summary>
        /// Count Banner item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p400Banner " + condition;
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
        /// Get All Banner Join Banner
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CustomJoin()
        {
            return await DbQueryAsync<object>("SELECT p400Banner.*, Person.* FROM p400Banner INNER JOIN Person on p400Banner.id = Person.Id");
        }

        public async Task<IEnumerable<E400Banner>> GetAllArragePositionAsync()
        {
            return await DbQueryAsync<E400Banner>("SELECT id,Image,Position FROM p400Banner ORDER BY Position");
        }
    }
}