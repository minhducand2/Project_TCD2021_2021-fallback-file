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
    public class D2100PromotionDataAccess : DbFactoryBase, ID2100PromotionDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D2100PromotionDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All Promotion Async
        /// </summary>
        /// <returns>IEnumerable<Promotion></returns>
        public async Task<IEnumerable<E2100Promotion>> GetAllAsync()
        {
            return await DbQueryAsync<E2100Promotion>("SELECT id,Name,PromotionCode,PercentCode,MoneyDiscount,StartDate,EndDate,Point FROM p2100Promotion");
        }

        /// <summary>
        /// Create Promotion Async
        /// </summary>
        /// <param name="promotion"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E2100Promotion promotion)
        {
            string sqlQuery = $@"INSERT INTO p2100Promotion(Name,PromotionCode,PercentCode,MoneyDiscount,StartDate,EndDate,Point)
                                 OUTPUT INSERTED.ID
                                 VALUES(@Name,@PromotionCode,@PercentCode,@MoneyDiscount,@StartDate,@EndDate,@Point);
                                  ";

            return await DbQuerySingleAsync<long>(sqlQuery, promotion);
        }

        /// <summary>
        /// Update Promotion Async
        /// </summary>
        /// <param name="promotion"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E2100Promotion promotion)
        {
            string sqlQuery = $@"UPDATE p2100Promotion SET Name=@Name,PromotionCode=@PromotionCode,PercentCode=@PercentCode,MoneyDiscount=@MoneyDiscount,StartDate=@StartDate,EndDate=@EndDate,Point=@Point
                                 WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, promotion);
        }

        /// <summary>
        /// Delete Promotion Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p2100Promotion
                                WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E2100Promotion> GetByIdAsync(object id)
        {
            string sqlQuery = $@"SELECT * FROM p2100Promotion
                                                    WHERE id = @id";
            return await DbQuerySingleAsync<E2100Promotion>(sqlQuery, new { id });
        }

        public async Task<E2100Promotion> GetByPromotionCodeAsync(object PromotionCode)
        {
            string sqlQuery = $@"SELECT * FROM p2100Promotion
                                 WHERE PromotionCode = @PromotionCode
                                  AND CURRENT_TIMESTAMP BETWEEN StartDate AND EndDate";
            return await DbQuerySingleAsync<E2100Promotion>(sqlQuery, new { PromotionCode });
        }

        /// <summary>
        /// Get Promotions Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E2100Promotion>> GetPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E2100Promotion> promotions;
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM p2100Promotion   " + urlQueryParameters.condition + "   ");
            query.Append("  ORDER BY id OFFSET " + urlQueryParameters.offset + " ROWS FETCH NEXT " + urlQueryParameters.limit + " ROWS ONLY");
             

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            promotions = await DbQueryAsync<E2100Promotion>(query.ToString(), parameters);

            return promotions;
        } 

        /// <summary>
        /// Count Promotion item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p2100Promotion " + condition;
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
        /// Get All Promotion Join Promotion
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CustomJoin()
        {
            return await DbQueryAsync<object>("SELECT p2100Promotion.*, Person.* FROM p2100Promotion INNER JOIN Person on p2100Promotion.id = Person.Id");
        }
    }
}