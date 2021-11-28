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
    public class D900ShopComboDetailDataAccess : DbFactoryBase, ID900ShopComboDetailDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D900ShopComboDetailDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All ShopComboDetail Async
        /// </summary>
        /// <returns>IEnumerable<ShopComboDetail></returns>
        public async Task<IEnumerable<E900ShopComboDetail>> GetAllAsync()
        {
            return await DbQueryAsync<E900ShopComboDetail>("SELECT id,IdShopCombo,IdShop FROM p900ShopComboDetail");
        }

        /// <summary>
        /// Create ShopComboDetail Async
        /// </summary>
        /// <param name="shopComboDetail"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E900ShopComboDetail shopComboDetail)
        {
            string sqlQuery = $@"INSERT INTO p900ShopComboDetail(IdShopCombo,IdShop)
                                OUTPUT INSERTED.ID
                                 VALUES(@IdShopCombo,@IdShop); ";

            return await DbQuerySingleAsync<long>(sqlQuery, shopComboDetail);
        }

        /// <summary>
        /// Update ShopComboDetail Async
        /// </summary>
        /// <param name="shopComboDetail"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E900ShopComboDetail shopComboDetail)
        {
            string sqlQuery = $@"UPDATE p900ShopComboDetail SET IdShopCombo=@IdShopCombo,IdShop=@IdShop
                                 WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, shopComboDetail);
        }

        /// <summary>
        /// Delete ShopComboDetail Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p900ShopComboDetail
                                WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E900ShopComboDetail> GetByIdAsync(object id)
        {
            string sqlQuery = $@"SELECT * FROM p900ShopComboDetail
                                                    WHERE id = @id";
            return await DbQuerySingleAsync<E900ShopComboDetail>(sqlQuery, new { id });
        }

        /// <summary>
        /// Get ShopComboDetails Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E900ShopComboDetail>> GetPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E900ShopComboDetail> shopComboDetails;
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM p900ShopComboDetail   " + urlQueryParameters.condition + "   ");
            query.Append("  ORDER BY id OFFSET " + urlQueryParameters.offset + " ROWS FETCH NEXT " + urlQueryParameters.limit + " ROWS ONLY");
 

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            shopComboDetails = await DbQueryAsync<E900ShopComboDetail>(query.ToString(), parameters);

            return shopComboDetails;
        } 

        /// <summary>
        /// Count ShopComboDetail item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p900ShopComboDetail " + condition;
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
        /// Get All ShopComboDetail Join ShopComboDetail
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CustomJoin()
        {
            return await DbQueryAsync<object>("SELECT p900ShopComboDetail.*, Person.* FROM p900ShopComboDetail INNER JOIN Person on p900ShopComboDetail.id = Person.Id");
        }
    }
}