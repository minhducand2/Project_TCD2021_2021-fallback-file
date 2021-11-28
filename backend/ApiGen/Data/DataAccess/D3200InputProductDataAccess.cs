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
    public class D3200InputProductDataAccess : DbFactoryBase, ID3200InputProductDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D3200InputProductDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All InputProduct Async
        /// </summary>
        /// <returns>IEnumerable<InputProduct></returns>
        public async Task<IEnumerable<E3200InputProduct>> GetAllAsync()
        {
            return await DbQueryAsync<E3200InputProduct>("SELECT id,IdShop,Note,Amount,CreatedAt,IdCity FROM p3200InputProduct ORDER BY CreatedAt DESC");
        }

        /// <summary>
        /// Create InputProduct Async
        /// </summary>
        /// <param name="inputProduct"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E3200InputProduct inputProduct)
        {
            string sqlQuery = $@"INSERT INTO p3200InputProduct(IdShop,Note,Amount,CreatedAt,IdCity)
                                 OUTPUT INSERTED.ID
                                 VALUES(@IdShop,@Note,@Amount,@CreatedAt,@IdCity);
                                  ";

            return await DbQuerySingleAsync<long>(sqlQuery, inputProduct);
        }

        /// <summary>
        /// Update InputProduct Async
        /// </summary>
        /// <param name="inputProduct"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E3200InputProduct inputProduct)
        {
            string sqlQuery = $@"UPDATE p3200InputProduct SET IdShop=@IdShop,Note=@Note,Amount=@Amount,CreatedAt=@CreatedAt,IdCity=@IdCity
                                 WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, inputProduct);
        }

        /// <summary>
        /// Delete InputProduct Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p3200InputProduct
                                WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E3200InputProduct> GetByIdAsync(object id)
        {
            string sqlQuery = $@"SELECT * FROM p3200InputProduct
                                                    WHERE id = @id";
            return await DbQuerySingleAsync<E3200InputProduct>(sqlQuery, new { id });
        }

        /// <summary>
        /// Get InputProducts Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E3200InputProduct>> GetPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E3200InputProduct> inputProducts;
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM p3200InputProduct   " + urlQueryParameters.condition + "   ");
            query.Append("  ORDER BY id OFFSET " + urlQueryParameters.offset + " ROWS FETCH NEXT " + urlQueryParameters.limit + " ROWS ONLY");
 

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            inputProducts = await DbQueryAsync<E3200InputProduct>(query.ToString(), parameters);

            return inputProducts;
        } 

        /// <summary>
        /// Count InputProduct item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p3200InputProduct " + condition;
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
        /// Get All InputProduct Join InputProduct
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CustomJoin()
        {
            return await DbQueryAsync<object>("SELECT p3200InputProduct.*, Person.* FROM p3200InputProduct INNER JOIN Person on p3200InputProduct.id = Person.Id");
        }
    }
}