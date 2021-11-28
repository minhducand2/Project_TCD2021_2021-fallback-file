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
    public class D800ShopComboDataAccess : DbFactoryBase, ID800ShopComboDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D800ShopComboDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All ShopCombo Async
        /// </summary>
        /// <returns>IEnumerable<ShopCombo></returns>
        public async Task<IEnumerable<E800ShopCombo>> GetAllAsync()
        {
            return await DbQueryAsync<E800ShopCombo>("SELECT id,Usd,Ship FROM p800ShopCombo");
        }

        /// <summary>
        /// Create ShopCombo Async
        /// </summary>
        /// <param name="shopCombo"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E800ShopCombo shopCombo)
        {
            string sqlQuery = $@"INSERT INTO p800ShopCombo(Usd,Ship)
                                OUTPUT INSERTED.ID
                                 VALUES(@Usd,@Ship);  ";

            return await DbQuerySingleAsync<long>(sqlQuery, shopCombo);
        }

        /// <summary>
        /// Update ShopCombo Async
        /// </summary>
        /// <param name="shopCombo"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E800ShopCombo shopCombo)
        {
            string sqlQuery = $@"UPDATE p800ShopCombo SET Usd=@Usd,Ship=@Ship
                                 WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, shopCombo);
        }

        /// <summary>
        /// Delete ShopCombo Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p800ShopCombo
                                WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E800ShopCombo> GetByIdAsync(object id)
        {
            string sqlQuery = $@"SELECT * FROM p800ShopCombo
                                                    WHERE id = @id";
            return await DbQuerySingleAsync<E800ShopCombo>(sqlQuery, new { id });
        }

        /// <summary>
        /// Get ShopCombos Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E800ShopCombo>> GetPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E800ShopCombo> shopCombos;
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM p800ShopCombo   " + urlQueryParameters.condition + "   ");
            query.Append("  ORDER BY id OFFSET " + urlQueryParameters.offset + " ROWS FETCH NEXT " + urlQueryParameters.limit + " ROWS ONLY");

 

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            shopCombos = await DbQueryAsync<E800ShopCombo>(query.ToString(), parameters);

            return shopCombos;
        } 

        /// <summary>
        /// Count ShopCombo item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p800ShopCombo " + condition;
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
        /// Get All ShopCombo Join ShopCombo
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CustomJoin()
        {
            return await DbQueryAsync<object>("SELECT p800ShopCombo.*, Person.* FROM p800ShopCombo INNER JOIN Person on p800ShopCombo.id = Person.Id");
        }
    }
}