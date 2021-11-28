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
    public class D2700ProductTypeDataAccess : DbFactoryBase, ID2700ProductTypeDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D2700ProductTypeDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All ProductType Async
        /// </summary>
        /// <returns>IEnumerable<ProductType></returns>
        public async Task<IEnumerable<E2700ProductType>> GetAllAsync()
        {
            return await DbQueryAsync<E2700ProductType>("SELECT id,Name FROM p2700ProductType");
        }

        /// <summary>
        /// Create ProductType Async
        /// </summary>
        /// <param name="productType"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E2700ProductType productType)
        {
            string sqlQuery = $@"INSERT INTO p2700ProductType(Name)
                                 OUTPUT INSERTED.ID
                                 VALUES(@Name);
                                  ";

            return await DbQuerySingleAsync<long>(sqlQuery, productType);
        }

        /// <summary>
        /// Update ProductType Async
        /// </summary>
        /// <param name="productType"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E2700ProductType productType)
        {
            string sqlQuery = $@"UPDATE p2700ProductType SET Name=@Name
                                 WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, productType);
        }

        /// <summary>
        /// Delete ProductType Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p2700ProductType
                                WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E2700ProductType> GetByIdAsync(object id)
        {
            string sqlQuery = $@"SELECT * FROM p2700ProductType
                                                    WHERE id = @id";
            return await DbQuerySingleAsync<E2700ProductType>(sqlQuery, new { id });
        }

        /// <summary>
        /// Get ProductTypes Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E2700ProductType>> GetPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E2700ProductType> productTypes;
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM p2700ProductType   " + urlQueryParameters.condition + "   ");
            query.Append("  ORDER BY id OFFSET " + urlQueryParameters.offset + " ROWS FETCH NEXT " + urlQueryParameters.limit + " ROWS ONLY");

 

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            productTypes = await DbQueryAsync<E2700ProductType>(query.ToString(), parameters);

            return productTypes;
        } 

        /// <summary>
        /// Count ProductType item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p2700ProductType " + condition;
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
        /// Get All ProductType Join ProductType
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CustomJoin()
        {
            return await DbQueryAsync<object>("SELECT p2700ProductType.*, Person.* FROM p2700ProductType INNER JOIN Person on p2700ProductType.id = Person.Id");
        }
    }
}