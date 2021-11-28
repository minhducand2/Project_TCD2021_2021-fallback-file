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
    public class D2900OrderDetailDataAccess : DbFactoryBase, ID2900OrderDetailDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D2900OrderDetailDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All OrderDetail Async
        /// </summary>
        /// <returns>IEnumerable<OrderDetail></returns>
        public async Task<IEnumerable<E2900OrderDetail>> GetAllAsync()
        {
            return await DbQueryAsync<E2900OrderDetail>("SELECT id,IdOrderShop,IdShop,Amount,CreatedAt,UpdatedAt FROM p2900OrderDetail");
        }

        /// <summary>
        /// Create OrderDetail Async
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E2900OrderDetail orderDetail)
        {
            string sqlQuery = $@"INSERT INTO p2900OrderDetail(IdOrderShop,IdShop,Amount,CreatedAt,UpdatedAt)
                                 OUTPUT INSERTED.ID
                                 VALUES(@IdOrderShop,@IdShop,@Amount,@CreatedAt,@UpdatedAt);
                                 ";

            return await DbQuerySingleAsync<long>(sqlQuery, orderDetail);
        }

        /// <summary>
        /// Update OrderDetail Async
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E2900OrderDetail orderDetail)
        {
            string sqlQuery = $@"UPDATE p2900OrderDetail SET IdOrderShop=@IdOrderShop,IdShop=@IdShop,Amount=@Amount,CreatedAt=@CreatedAt,UpdatedAt=@UpdatedAt
                                 WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, orderDetail);
        }

        /// <summary>
        /// Delete OrderDetail Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p2900OrderDetail
                                WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        public async Task<bool> UpdateAmountMinusAsync(object id)
        {
            string sqlQuery = $@"UPDATE p2900OrderDetail SET Amount = Amount - 1
                                WHERE id IN(" + id + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        public async Task<bool> UpdateAmountPlusAsync(object id)
        {
            string sqlQuery = $@"UPDATE p2900OrderDetail SET Amount = Amount + 1
                                WHERE id IN(" + id + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E2900OrderDetail> GetByIdAsync(object id)
        {
            string sqlQuery = $@"SELECT * FROM p2900OrderDetail
                                                    WHERE id = @id";
            return await DbQuerySingleAsync<E2900OrderDetail>(sqlQuery, new { id });
        }

        /// <summary>
        /// Get OrderDetails Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E2900OrderDetail>> GetPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E2900OrderDetail> orderDetails;
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM p2900OrderDetail "+ urlQueryParameters .condition+" ");
            query.Append("  ORDER BY id OFFSET " + urlQueryParameters.offset + " ROWS FETCH NEXT " + urlQueryParameters.limit + " ROWS ONLY");


            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit, 
                condition = urlQueryParameters.condition
            };

            orderDetails = await DbQueryAsync<E2900OrderDetail>(query.ToString(), parameters);

            return orderDetails;
        } 

        /// <summary>
        /// Count OrderDetail item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p2900OrderDetail " + condition;
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
        /// Get All OrderDetail Join OrderDetail
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CustomJoin()
        {
            return await DbQueryAsync<object>("SELECT p2900OrderDetail.*, Person.* FROM p2900OrderDetail INNER JOIN Person on p2900OrderDetail.id = Person.Id");
        }

        public async Task<IEnumerable<object>> CustomJoinAmountDate(ParametersDateTime parameters)
        {
            string sqlQuery = @"SELECT SUM(Amount) as Amount FROM p2900OrderDetail WHERE (CreatedAt BETWEEN
                                (CASE WHEN @startDate = '0' AND @endDate = '0' THEN '1753-01-01 00:00:00'
                                WHEN @startDate != '0' AND @endDate = '0' THEN  @startDate1 
                                WHEN @startDate = '0' AND @endDate != '0' THEN  '1753-01-01 00:00:00' 
                                ELSE @startDate1 END) 
                                AND                             
                                (CASE WHEN @startDate = '0' AND @endDate = '0' THEN '9999-12-31 23:59:59' 
                                WHEN @startDate != '0' AND @endDate = '0' THEN '9999-12-31 23:59:59' 
                                WHEN @startDate = '0' AND @endDate != '0' THEN @endDate1
                                ELSE @endDate1 END))";
            return await DbQueryAsync<object>(sqlQuery, parameters);
        }

        public async Task<IEnumerable<object>> CustomJoinMoneyDate(ParametersDateTime parameters)
        {
            string sqlQuery = @"SELECT t2.PriceCurrent,t2.PriceOrigin,t3.TotalPrice,SUM(t1.Amount) as Amount FROM p2900orderdetail t1
                                INNER JOIN p700shop t2 ON t1.IdShop = t2.id
                                INNER JOIN p2800ordershop t3 ON t1.IdOrderShop = t3.id WHERE (t3.CreatedAt BETWEEN
                                (CASE WHEN @startDate = '0' AND @endDate = '0' THEN '1753-01-01 00:00:00'
                                WHEN @startDate != '0' AND @endDate = '0' THEN  @startDate1 
                                WHEN @startDate = '0' AND @endDate != '0' THEN  '1753-01-01 00:00:00' 
                                ELSE @startDate1 END) 
                                AND                             
                                (CASE WHEN @startDate = '0' AND @endDate = '0' THEN '9999-12-31 23:59:59' 
                                WHEN @startDate != '0' AND @endDate = '0' THEN '9999-12-31 23:59:59' 
                                WHEN @startDate = '0' AND @endDate != '0' THEN @endDate1
                                ELSE @endDate1 END))
                                AND t3.IdOrderStatus = '4'
                                AND t3.IdPaymentStatus = '2'
                                GROUP BY t1.IdOrderShop,t2.PriceCurrent,t2.PriceOrigin,t3.TotalPrice";
            return await DbQueryAsync<object>(sqlQuery, parameters);
        }

        public async Task<IEnumerable<object>> CustomJoinMoneyDay(ParametersDateTime parameters)
        {
            string sqlQuery = @"SELECT t2.PriceCurrent,t2.PriceOrigin,t3.TotalPrice,SUM(t1.Amount) as Amount FROM p2900orderdetail t1
                                INNER JOIN p700shop t2 ON t1.IdShop = t2.id
                                INNER JOIN p2800ordershop t3 ON t1.IdOrderShop = t3.id WHERE 
                                t3.CreatedAt BETWEEN  @startDate AND @endDate
                                AND t3.IdOrderStatus = '4'
                                AND t3.IdPaymentStatus = '2'
                                GROUP BY t1.IdOrderShop,t2.PriceCurrent,t2.PriceOrigin,t3.TotalPrice";
            return await DbQueryAsync<object>(sqlQuery, parameters);
        }

        public async Task<IEnumerable<object>> SelectWithIdOderShop(ParametersCheckAmount paramInput)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT t1.IdShop,SUM(t1.Amount) as UserAmount,t2.Amount as WareHouseAmount");
            query.Append(" FROM p2900OrderDetail t1 ");
            query.Append(" INNER JOIN p3300warehouse t2 ON t1.IdShop = t2.IdShop ");
            query.Append(" WHERE t1.IdOrderShop = '" + paramInput.IdOrderShop + "' ");
            query.Append(" AND t2.IdCity = '" + paramInput.IdCity + "' ");
            query.Append(" GROUP BY t1.IdShop,t2.Amount,t2.IdCity	 ");

            return await DbQueryAsync<object>(query.ToString(), new { paramInput });
        }

        public async Task<object> GetByUserManagerProducDetailtAsync(object id)
        {

            StringBuilder query = new StringBuilder();
            query.Append("SELECT t1.*, t2.TotalPrice,t2.PromotionCode FROM p2900orderdetail t1");
            query.Append(" INNER JOIN p2800ordershop t2 ON t1.IdOrderShop = t2.id");
            query.Append(" WHERE t1.IdOrderShop = '" + id + "' "); 

            return await DbQueryAsync<object>(query.ToString(), new { });
        }

        public async Task<IEnumerable<object>> SelectWithIdOderShopReset(object id)
        {
            string sqlQuery = $@"SELECT * FROM p2900OrderDetail 
                                    WHERE IdOrderShop = @id";
            return await DbQueryAsync<object>(sqlQuery, new { id });
        }

        public async Task<long> CreateOrderRessetAsync(object orderDetail)
        {

            StringBuilder query = new StringBuilder();
            query.Append("INSERT INTO p2900OrderDetail(IdOrderShop,IdShop,Amount");
            query.Append("VALUES(@IdOrderShop, @IdShop,@Amount)"); 

            return await DbQuerySingleAsync<long>(query.ToString(), orderDetail);
        }

        public async Task<bool> RevierStarOrder(ParametersReviewStar parameters)
        {
            StringBuilder query = new StringBuilder();
            query.Append("UPDATE p2900OrderDetail SET Star = "+ parameters .number+ "");
            query.Append(" WHERE id = " + parameters.idOrderStar + " ");

            var orderReview = await DbExecuteAsync<bool>(query.ToString(), new { });

            if (orderReview)
            {
                StringBuilder query1 = new StringBuilder();
                query1.Append("SELECT AVG(Star) as Star FROM p2900orderdetail");
                query1.Append(" WHERE IdShop = " + parameters.idShopStar + " ");
                var numberStar = await DbQuerySingleAsync<NumberStar>(query1.ToString(), new { });
                if(numberStar != null)
                {
                    StringBuilder query2 = new StringBuilder();
                    query2.Append("UPDATE p700shop SET Star = " + numberStar.Star + "");
                    query2.Append(" WHERE id = " + parameters.idShopStar + " ");

                    var updateStar = await DbExecuteAsync<bool>(query2.ToString(), new { });
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}