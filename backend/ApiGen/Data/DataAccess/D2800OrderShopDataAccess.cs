using ApiGen.Data.Entity;
using AutoMapper;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public class D2800OrderShopDataAccess : DbFactoryBase, ID2800OrderShopDataAccess
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;

        public D2800OrderShopDataAccess(IConfiguration config, ILogger<dynamic> logger, IMapper mapper) : base(config)
        {
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get All OrderShop Async
        /// </summary>
        /// <returns>IEnumerable<OrderShop></returns>
        public async Task<IEnumerable<E2800OrderShop>> GetAllAsync()
        {
            return await DbQueryAsync<E2800OrderShop>("SELECT id,IdProductType,IdUser,IdOrderStatus,IdCity,IdDistrict,IdPaymentStatus,IdPaymentType,TotalPrice,PromotionCode,Name,Email,Phone,Address,Note,CreatedAt,UpdatedAt FROM p2800OrderShop");
        }

        /// <summary>
        /// Create OrderShop Async
        /// </summary>
        /// <param name="orderShop"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E2800OrderShop orderShop)
        {
            string sqlQuery = $@"INSERT INTO p2800OrderShop(IdProductType,IdUser,IdOrderStatus,IdCity,IdDistrict,IdPaymentStatus,IdPaymentType,TotalPrice,PromotionCode,Name,Email,Phone,Address,Note,CreatedAt,UpdatedAt)
                                 OUTPUT INSERTED.ID
                                 VALUES(@IdProductType,@IdUser,@IdOrderStatus,@IdCity,@IdDistrict,@IdPaymentStatus,@IdPaymentType,@TotalPrice,@PromotionCode,@Name,@Email,@Phone,@Address,@Note,@CreatedAt,@UpdatedAt);
                                  ";

            return await DbQuerySingleAsync<long>(sqlQuery, orderShop);
        }

        /// <summary>
        /// Update OrderShop Async
        /// </summary>
        /// <param name="orderShop"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E2800OrderShop orderShop)
        {
            string sqlQuery = $@"UPDATE p2800OrderShop SET IdProductType=@IdProductType,IdUser=@IdUser,IdOrderStatus=@IdOrderStatus,IdCity=@IdCity,IdDistrict=@IdDistrict,IdPaymentStatus=@IdPaymentStatus,IdPaymentType=@IdPaymentType,TotalPrice=@TotalPrice,PromotionCode=@PromotionCode,Name=@Name,Email=@Email,Phone=@Phone,Address=@Address,Note=@Note,CreatedAt=@CreatedAt,UpdatedAt=@UpdatedAt
                                 WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, orderShop);
        }
        public async Task<bool> UpdateTotalPriceAsync(ParametersOderShopCart parameters)
        { 
            StringBuilder query = new StringBuilder();
            query.Append("UPDATE p2800OrderShop SET TotalPrice ='" + parameters.TotalPrice + "' ");
            query.Append("WHERE id ='" + parameters.id + "' ");

            return await DbExecuteAsync<bool>(query.ToString(), new { });
        }
        public async Task<bool> UpdatePayAsync(ParametersOderShopCart orderShop)
        {
            StringBuilder query = new StringBuilder();
            query.Append("UPDATE p2800OrderShop SET  ");
            query.Append(" IdOrderStatus=2,IdCity='"+orderShop.IdCity+ "', IdDistrict='" + orderShop.IdDistrict + "',IdPaymentStatus=1, ");
            query.Append(" IdPaymentType='" + orderShop.IdPaymentType + "',TotalPrice='" + orderShop.TotalPrice + "',PromotionCode='" + orderShop.PromotionCode + "',Name='" + orderShop.Name + "', ");
            query.Append(" Email='" + orderShop.Email + "',Phone='" + orderShop.Phone + "',Address='" + orderShop.Address + "',Note='" + orderShop.Note + "', Point='" + orderShop.Point + "' ");
            query.Append(" WHERE id ='" + orderShop.id + "' ");

            var Order = await DbExecuteAsync<bool>(query.ToString(), new { });
            if (Order)
            {
                StringBuilder query1 = new StringBuilder();
                query1.Append("UPDATE p2000User SET Point = '" + orderShop.PointUser + "' ");
                query1.Append("  WHERE id = '" + orderShop.IdUser + "' ");
                return await DbExecuteAsync<bool>(query1.ToString(), new { });
            }
            else
            {
                return false;
            }
            return true;
        }



        /// <summary>
        /// Delete OrderShop Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p2800OrderShop
                                WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E2800OrderShop> GetByIdAsync(object id)
        {
            string sqlQuery = $@"SELECT * FROM p2800OrderShop
                                                    WHERE id = @id";
            return await DbQuerySingleAsync<E2800OrderShop>(sqlQuery, new { id });
        }

        /// <summary>
        /// Get OrderShops Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E2800OrderShop>> GetPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E2800OrderShop> orderShops;
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM p2800OrderShop   " + urlQueryParameters.condition + "   ");
            query.Append("  ORDER BY id DESC OFFSET " + urlQueryParameters.offset + " ROWS FETCH NEXT " + urlQueryParameters.limit + " ROWS ONLY");

 

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            orderShops = await DbQueryAsync<E2800OrderShop>(query.ToString(), parameters);

            return orderShops;
        } 

        /// <summary>
        /// Count OrderShop item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p2800OrderShop " + condition;
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
        /// Get All OrderShop Join OrderShop
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CustomJoin()
        {
            return await DbQueryAsync<object>("SELECT p2800OrderShop.*, Person.* FROM p2800OrderShop INNER JOIN Person on p2800OrderShop.id = Person.Id");
        }

        public async Task<IEnumerable<object>> CustomJoinOrder1Date(ParametersDateTime parameters)
        {
            string sqlQuery = @"SELECT COUNT(1) as Amount FROM p2800OrderShop WHERE (CreatedAt BETWEEN
                                (CASE WHEN @startDate = '0' AND @endDate = '0' THEN '1753-01-01 00:00:00'
                                WHEN @startDate != '0' AND @endDate = '0' THEN  @startDate1 
                                WHEN @startDate = '0' AND @endDate != '0' THEN  '1753-01-01 00:00:00' 
                                ELSE @startDate1 END) 
                                AND                             
                                (CASE WHEN @startDate = '0' AND @endDate = '0' THEN '9999-12-31 23:59:59' 
                                WHEN @startDate != '0' AND @endDate = '0' THEN '9999-12-31 23:59:59' 
                                WHEN @startDate = '0' AND @endDate != '0' THEN @endDate1
                                ELSE @endDate1 END))
                                AND IdOrderStatus ='1'";
            return await DbQueryAsync<object>(sqlQuery, parameters);
        }

        public async Task<IEnumerable<object>> CustomJoinOrder2Date(ParametersDateTime parameters)
        {
            string sqlQuery = @"SELECT COUNT(1) as Amount FROM p2800OrderShop WHERE (CreatedAt BETWEEN
                                (CASE WHEN @startDate = '0' AND @endDate = '0' THEN '1753-01-01 00:00:00'
                                WHEN @startDate != '0' AND @endDate = '0' THEN  @startDate1 
                                WHEN @startDate = '0' AND @endDate != '0' THEN  '1753-01-01 00:00:00' 
                                ELSE @startDate1 END) 
                                AND                             
                                (CASE WHEN @startDate = '0' AND @endDate = '0' THEN '9999-12-31 23:59:59' 
                                WHEN @startDate != '0' AND @endDate = '0' THEN '9999-12-31 23:59:59' 
                                WHEN @startDate = '0' AND @endDate != '0' THEN @endDate1
                                ELSE @endDate1 END))
                                AND IdOrderStatus ='2'";
            return await DbQueryAsync<object>(sqlQuery, parameters);
        }

        public async Task<IEnumerable<object>> CustomJoinOrder3Date(ParametersDateTime parameters)
        {
            string sqlQuery = @"SELECT COUNT(1) as Amount FROM p2800OrderShop WHERE (CreatedAt BETWEEN
                                (CASE WHEN @startDate = '0' AND @endDate = '0' THEN '1753-01-01 00:00:00'
                                WHEN @startDate != '0' AND @endDate = '0' THEN  @startDate1 
                                WHEN @startDate = '0' AND @endDate != '0' THEN  '1753-01-01 00:00:00' 
                                ELSE @startDate1 END) 
                                AND                             
                                (CASE WHEN @startDate = '0' AND @endDate = '0' THEN '9999-12-31 23:59:59' 
                                WHEN @startDate != '0' AND @endDate = '0' THEN '9999-12-31 23:59:59' 
                                WHEN @startDate = '0' AND @endDate != '0' THEN @endDate1
                                ELSE @endDate1 END))
                                AND IdOrderStatus ='3'";
            return await DbQueryAsync<object>(sqlQuery, parameters);
        }

        public async Task<IEnumerable<object>> CustomJoinOrder4Date(ParametersDateTime parameters)
        {
            string sqlQuery = @"SELECT COUNT(1) as Amount FROM p2800OrderShop WHERE (CreatedAt BETWEEN
                                (CASE WHEN @startDate = '0' AND @endDate = '0' THEN '1753-01-01 00:00:00'
                                WHEN @startDate != '0' AND @endDate = '0' THEN  @startDate1 
                                WHEN @startDate = '0' AND @endDate != '0' THEN  '1753-01-01 00:00:00' 
                                ELSE @startDate1 END) 
                                AND                             
                                (CASE WHEN @startDate = '0' AND @endDate = '0' THEN '9999-12-31 23:59:59' 
                                WHEN @startDate != '0' AND @endDate = '0' THEN '9999-12-31 23:59:59' 
                                WHEN @startDate = '0' AND @endDate != '0' THEN @endDate1
                                ELSE @endDate1 END))
                                AND IdOrderStatus ='4'";
            return await DbQueryAsync<object>(sqlQuery, parameters);
        }

        public async Task<IEnumerable<object>> CustomJoinOrder5Date(ParametersDateTime parameters)
        {
            string sqlQuery = @"SELECT COUNT(1) as Amount FROM p2800OrderShop WHERE (CreatedAt BETWEEN
                                (CASE WHEN @startDate = '0' AND @endDate = '0' THEN '1753-01-01 00:00:00'
                                WHEN @startDate != '0' AND @endDate = '0' THEN  @startDate1 
                                WHEN @startDate = '0' AND @endDate != '0' THEN  '1753-01-01 00:00:00' 
                                ELSE @startDate1 END) 
                                AND                             
                                (CASE WHEN @startDate = '0' AND @endDate = '0' THEN '9999-12-31 23:59:59' 
                                WHEN @startDate != '0' AND @endDate = '0' THEN '9999-12-31 23:59:59' 
                                WHEN @startDate = '0' AND @endDate != '0' THEN @endDate1
                                ELSE @endDate1 END))
                                AND IdOrderStatus ='5'";
            return await DbQueryAsync<object>(sqlQuery, parameters);
        }

        public async Task<IEnumerable<object>> CustomJoinOrderAllDate(ParametersDateTime parameters)
        {
            string sqlQuery = @"SELECT COUNT(1) as Amount FROM p2800OrderShop WHERE (CreatedAt BETWEEN
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

        public async Task<object> GetByIdUserCountCartAsync(object IdUser)
        {
            string sqlQuery = $@"SELECT COUNT(1) as count FROM p2900orderDetail
                            INNER JOIN p2800ordershop 
                            ON p2900orderDetail.IdOrderShop = p2800ordershop.id
                            WHERE p2800ordershop.IdUser = @IdUser
                                AND p2800ordershop.IdOrderStatus = 1";
            return await DbQuerySingleAsync<object>(sqlQuery, new { IdUser });
        }

        public async Task<bool> GetOrderShopWithUser(ParametersOrderShop parameters)
        {
           

            StringBuilder query = new StringBuilder();
            query.Append("SELECT id FROM p2800ordershop ");
            query.Append(" WHERE IdOrderStatus = '1'");
            query.Append(" AND IdUser = '"+parameters.IdUser+"'");
            var IdOrderShop = await DbQuerySingleAsync<IdOrderShop>(query.ToString(), new {  });

            //int numberOrderShop = IdOrderShop.Count();
            //check IdOderShop with IdUser
            if (IdOrderShop != null)
            {
                StringBuilder query1 = new StringBuilder();
                query1.Append("SELECT id FROM p2900orderDetail ");
                query1.Append(" WHERE IdOrderShop = '"+ IdOrderShop.id + "'");
                query1.Append(" AND IdShop = '" + parameters.IdShop + "'");

                var IdOrderDetail = await DbQuerySingleAsync<IdOrderShop>(query1.ToString(), new { });
                //int numberOrderDetail = IdOrderDetail.Count();
                // update amount pOrderDetail
                if (IdOrderDetail != null)
                {
                    StringBuilder query2 = new StringBuilder();
                    query2.Append("UPDATE p2900orderDetail SET ");
                    query2.Append(" Amount = Amount + " + parameters.Amount + "");
                    query2.Append(" WHERE id = " + IdOrderDetail.id + " ");

                    //return await DbQueryAsync<E2900OrderDetail>(query2.ToString(), parameters);
                    return await DbExecuteAsync<bool>(query2.ToString(), new {});
                }
                // inser amount pOrderDetail
                else
                {
                    StringBuilder query3 = new StringBuilder();
                    query3.Append("INSERT INTO p2900OrderDetail(IdOrderShop,IdShop,Amount)  OUTPUT INSERTED.ID ");
                    query3.Append("  VALUES("+ IdOrderShop.id + ", "+parameters.IdShop + ", " + parameters.Amount + ")");
                    //return await DbQueryAsync<E2900OrderDetail>(query3.ToString(), parameters);
                    return await DbExecuteAsync<bool>(query3.ToString(), new { });
                }
            }
            else
            {
                //insert p2800ordershop and p2900OrderDetail
                StringBuilder query4 = new StringBuilder();
                query4.Append("INSERT INTO p2800OrderShop (IdUser,IdOrderStatus,IdPaymentStatus)");
                query4.Append(" OUTPUT INSERTED.ID");
                query4.Append(" VALUES("+parameters.IdUser+",1,1)");

                var IdOrderShopNew = await DbQuerySingleAsync<long>(query4.ToString(), new { }); 
                StringBuilder query5 = new StringBuilder();
                query5.Append("INSERT INTO p2900OrderDetail(IdOrderShop,IdShop,Amount)  OUTPUT INSERTED.ID ");
                query5.Append("  VALUES(" + IdOrderShopNew + ", " + parameters.IdShop + ", " + parameters.Amount + ")");
                //return await DbQueryAsync<E2900OrderDetail>(query5.ToString(), parameters); 
                return await DbExecuteAsync<bool>(query5.ToString(), new { });
            }

        }
        public async Task<object> GetByUserCartAsync(object id)
        {

            StringBuilder query = new StringBuilder();
            query.Append("SELECT t2.id as idOderDetail, t2.IdOrderShop, t2.IdShop, t2.Amount FROM p2800ordershop t1 ");
            query.Append("INNER JOIN p2900orderdetail t2 ON t1.id =  t2.IdOrderShop ");          
            query.Append("WHERE t1.IdUser = '"+id+"' ");
            query.Append("AND t1.IdOrderStatus = 1 "); 

            return await DbQueryAsync<object>(query.ToString(), new { });
        }

        public async Task<object> GetByUserManagerProductAsync(object id)
        {

            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM p2800ordershop"); 
            query.Append(" WHERE IdUser = '" + id + "' ");
            query.Append("AND IdOrderStatus IN(1,2,3) ");

            return await DbQueryAsync<object>(query.ToString(), new { });
        }


        public async Task<object> GetByUserOrderReceivedAsync(object id)
        {

            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM p2800ordershop");
            query.Append(" WHERE IdUser = '" + id + "' ");
            query.Append("AND IdOrderStatus IN(4) ");

            return await DbQueryAsync<object>(query.ToString(), new { });
        }

        public async Task<object> GetByUserOrderRefuseAsync(object id)
        {

            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM p2800ordershop");
            query.Append(" WHERE IdUser = '" + id + "' ");
            query.Append("AND IdOrderStatus IN(5) ");

            return await DbQueryAsync<object>(query.ToString(), new { });
        }

        public async Task<bool> CancelOrderAsync(object id)
        {

            StringBuilder query = new StringBuilder();
            query.Append("UPDATE p2800OrderShop SET  ");
            query.Append(" IdOrderStatus= 5"); 
            query.Append(" WHERE id ='" + id + "' ");

            return await DbExecuteAsync<bool>(query.ToString(), new { });
        }

        public async Task<bool> GetByOrederShopResetAsync(ParametersOrderShopReset parameters)
        { 
            StringBuilder query = new StringBuilder();
            query.Append("SELECT id FROM p2800ordershop ");
            query.Append(" WHERE IdOrderStatus = '1'");
            query.Append(" AND IdUser = '" + parameters.IdUser + "'");
            var IdOrderShop = await DbQuerySingleAsync<IdOrderShop>(query.ToString(), new { });

            StringBuilder query1 = new StringBuilder();
            query1.Append("SELECT * FROM p2900OrderDetail ");
            query1.Append(" WHERE IdOrderShop = '"+ parameters.id+"'");

            var OrderDeatilReset = await DbQueryAsync<ParametersOrderShop>(query1.ToString(), new { }); 
          
            //check IdOderShop with IdUser
            if (IdOrderShop != null)
            {
               foreach(ParametersOrderShop item in OrderDeatilReset)
                { 

                    StringBuilder query2 = new StringBuilder();
                    query2.Append("SELECT id FROM p2900orderDetail ");
                    query2.Append(" WHERE IdOrderShop = '" + IdOrderShop.id + "'");
                    query2.Append(" AND IdShop = '" + item.IdShop + "'");

                    var IdOrderDetail = await DbQuerySingleAsync<IdOrderShop>(query2.ToString(), new { });
                 
                    // update amount pOrderDetail
                    if (IdOrderDetail != null)
                    {
                        StringBuilder query3 = new StringBuilder();
                        query3.Append("UPDATE p2900orderDetail SET ");
                        query3.Append(" Amount = Amount + " + item.Amount + "");
                        query3.Append(" WHERE id = " + IdOrderDetail.id + " "); 
                        
                        var InsertOrdetDetail = await DbExecuteAsync<bool>(query3.ToString(), new { });
                    }
                    // inser amount pOrderDetail
                    else
                    {
                        StringBuilder query4 = new StringBuilder();
                        query4.Append("INSERT INTO p2900OrderDetail(IdOrderShop,IdShop,Amount) ");
                        query4.Append("  VALUES(" + IdOrderShop.id + ", " + item.IdShop + ", " + item.Amount + ")");
                        //return await DbQueryAsync<E2900OrderDetail>(query3.ToString(), parameters);
                        var InsertOrdetDetail = await DbExecuteAsync<bool>(query4.ToString(), new { });
                    }
                }
                return true;
            }
            else
            {
                //insert p2800ordershop and p2900OrderDetail
                StringBuilder query5 = new StringBuilder();
                query5.Append("INSERT INTO p2800OrderShop (IdUser,IdOrderStatus,IdPaymentStatus)");
                query5.Append(" OUTPUT INSERTED.ID");
                query5.Append(" VALUES(" + parameters.IdUser + ",1,1)");

                var IdOrderShopNew = await DbQuerySingleAsync<IdOrderShop>(query5.ToString(), new { });

                foreach (ParametersOrderShop item in OrderDeatilReset)
                {
                    StringBuilder query3 = new StringBuilder();
                    query3.Append("INSERT INTO p2900OrderDetail(IdOrderShop,IdShop,Amount)");
                    query3.Append(" VALUES('" + IdOrderShopNew.id + "', '" + item.IdShop + "','" + item.Amount + "')");
                    var InsertOrdetDetail = await DbQuerySingleAsync<long>(query3.ToString(), new { });
                }

                return true;
            }

        }



    }
}