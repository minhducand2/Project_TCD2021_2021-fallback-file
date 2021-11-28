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
    public class D2600DistrictDataAccess : DbFactoryBase, ID2600DistrictDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D2600DistrictDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All District Async
        /// </summary>
        /// <returns>IEnumerable<District></returns>
        public async Task<IEnumerable<E2600District>> GetAllAsync()
        {
            return await DbQueryAsync<E2600District>("SELECT id,IdCity,Name FROM p2600District");
        }

        /// <summary>
        /// Create District Async
        /// </summary>
        /// <param name="district"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E2600District district)
        {
            string sqlQuery = $@"INSERT INTO p2600District(IdCity,Name)
                                 OUTPUT INSERTED.ID
                                 VALUES(@IdCity,@Name);
                                  ";

            return await DbQuerySingleAsync<long>(sqlQuery, district);
        }

        /// <summary>
        /// Update District Async
        /// </summary>
        /// <param name="district"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E2600District district)
        {
            string sqlQuery = $@"UPDATE p2600District SET IdCity=@IdCity,Name=@Name
                                 WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, district);
        }

        /// <summary>
        /// Delete District Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p2600District
                                WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E2600District> GetByIdAsync(object id)
        {
            string sqlQuery = $@"SELECT * FROM p2600District
                                                    WHERE id = @id";
            return await DbQuerySingleAsync<E2600District>(sqlQuery, new { id });
        }

        public async Task<object> GetByIdCityAsync(object id)
        {
            string sqlQuery = $@"SELECT * FROM p2600District
                                                    WHERE IdCity = @id";
            var districts = await DbQueryAsync<E2600District>(sqlQuery, new { id });
            return districts;
        }

        /// <summary>
        /// Get Districts Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E2600District>> GetPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E2600District> districts;
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM p2600District   " + urlQueryParameters.condition + "   ");
            query.Append("  ORDER BY id OFFSET " + urlQueryParameters.offset + " ROWS FETCH NEXT " + urlQueryParameters.limit + " ROWS ONLY");

             

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            districts = await DbQueryAsync<E2600District>(query.ToString(), parameters);

            return districts;
        } 

        /// <summary>
        /// Count District item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p2600District " + condition;
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
        /// Get All District Join District
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CustomJoin()
        {
            return await DbQueryAsync<object>("SELECT p2600District.*, Person.* FROM p2600District INNER JOIN Person on p2600District.id = Person.Id");
        }
    }
}