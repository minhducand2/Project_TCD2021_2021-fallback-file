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
    public class D700ShopDataAccess : DbFactoryBase, ID700ShopDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D700ShopDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All Shop Async
        /// </summary>
        /// <returns>IEnumerable<Shop></returns>
        public async Task<IEnumerable<E700Shop>> GetAllAsync()
        {
            return await DbQueryAsync<E700Shop>("SELECT id,Title,Thumbnail,Description,PriceOrigin,PriceCurrent,PricePromotion,Star,Images,Video,IdShopCategories,Point FROM p700Shop");
        }

        /// <summary>
        /// Create Shop Async
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E700Shop shop)
        {
            string sqlQuery = $@"INSERT INTO p700Shop(Title,Thumbnail,Description,PriceOrigin,PriceCurrent,PricePromotion,Star,Images,Video,IdShopCategories,Point)
                                OUTPUT INSERTED.ID
                                 VALUES(@Title,@Thumbnail,@Description,@PriceOrigin,@PriceCurrent,@PricePromotion,@Star,@Images,@Video,@IdShopCategories,@Point);";

            return await DbQuerySingleAsync<long>(sqlQuery, shop);
        }

        /// <summary>
        /// Update Shop Async
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E700Shop shop)
        {
            string sqlQuery = $@"UPDATE p700Shop SET Title=@Title,Thumbnail=@Thumbnail,Description=@Description,PriceOrigin=@PriceOrigin,PriceCurrent=@PriceCurrent,PricePromotion=@PricePromotion,Star=@Star,Images=@Images,Video=@Video,IdShopCategories=@IdShopCategories,Point=@Point
                                 WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, shop);
        }

        /// <summary>
        /// Delete Shop Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p700Shop
                                WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E700Shop> GetByIdAsync(object id)
        {
            string sqlQuery = $@"SELECT * FROM p700Shop
                                                    WHERE id = @id";
            return await DbQuerySingleAsync<E700Shop>(sqlQuery, new { id });
        }

        /// <summary>
        /// Get Shops Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E700Shop>> GetPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E700Shop> shops;
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM p700Shop   " + urlQueryParameters.condition + "   ");
            query.Append("  ORDER BY id OFFSET " + urlQueryParameters.offset + " ROWS FETCH NEXT " + urlQueryParameters.limit + " ROWS ONLY");
 

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            shops = await DbQueryAsync<E700Shop>(query.ToString(), parameters);

            return shops;
        } 

        /// <summary>
        /// Count Shop item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p700Shop " + condition;
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
        /// Get All Shop Join Shop
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CustomJoin()
        {
            return await DbQueryAsync<object>("SELECT p700Shop.*, Person.* FROM p700Shop INNER JOIN Person on p700Shop.id = Person.Id");
        }

        public async Task<IEnumerable<object>> CustomJoinTopShop()
        {
            string sqlQuery = @"SELECT t1.IdShop,t2.Thumbnail,t2.Title,t3.Name, Sum(t1.Amount) as totalAmount  
                                    FROM p2900orderdetail t1
                                    INNER JOIN p700shop t2 ON t1.IdShop = t2.id
                                    INNER JOIN p1100shopcategories t3 ON t2.IdShopCategories = t3.id
                                    GROUP BY t1.IdShop,t2.Title,t2.Thumbnail,t3.Name
                                    ORDER BY totalAmount DESC
                                    OFFSET 0 ROWS
								    FETCH NEXT 10 ROWS ONLY";
            return await DbQueryAsync<object>(sqlQuery);
        }
        //Get Product Ramdom limit 4
        public async Task<IEnumerable<E700Shop>> GetAllRamdomLimit4Async()
        {
            return await DbQueryAsync<E700Shop>("SELECT * FROM p700Shop ORDER BY id DESC OFFSET 0 ROWS FETCH NEXT 4 ROWS ONLY");
        }
        //Get Product Ramdom limit 4 Order By Id
        public async Task<IEnumerable<E700Shop>> GetAllRamdomLimit4AsyncSugest()
        {
            return await DbQueryAsync<E700Shop>("SELECT * FROM p700Shop ORDER BY NEWID() OFFSET 0 ROWS FETCH NEXT 4 ROWS ONLY");
        }
        //Get Product Ramdom limit 8 Order By Id with PricePromotion > 0
        public async Task<IEnumerable<E700Shop>> GetAllRamdomLimit8AsyncSugest()
        {
            return await DbQueryAsync<E700Shop>("SELECT * FROM p700Shop WHERE PricePromotion > 0 ORDER BY NEWID() OFFSET 0 ROWS FETCH NEXT 8 ROWS ONLY");
        }

        public async Task<IEnumerable<E700Shop>> GetProductListAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E700Shop> shops;
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT * FROM p700Shop");
            query.Append(" WHERE IdShopCategories = "+ urlQueryParameters.id+"");
            query.Append(" ORDER BY " + urlQueryParameters.condition + "");
            query.Append(" OFFSET " + urlQueryParameters.offset + " ROWS FETCH NEXT "+ urlQueryParameters.limit + " ROWS ONLY"); 


            StringBuilder query0 = new StringBuilder();
            query0.Append(" SELECT * FROM p700Shop"); 
            query0.Append(" ORDER BY " + urlQueryParameters.condition + "");
            query0.Append(" OFFSET " + urlQueryParameters.offset + " ROWS FETCH NEXT " + urlQueryParameters.limit + " ROWS ONLY");

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            if(urlQueryParameters.id == 0)
            {
                shops = await DbQueryAsync<E700Shop>(query0.ToString(), parameters);
            }
            else
            {
                shops = await DbQueryAsync<E700Shop>(query.ToString(), parameters);
            } 
            return shops;
        }

        public async Task<IEnumerable<object>> GetProductListCountAsync(UrlQueryParameters urlQueryParameters)
        {
            
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT COUNT(1) as count FROM p700Shop");
            query.Append(" WHERE IdShopCategories = " + urlQueryParameters.id + ""); 

            StringBuilder query0 = new StringBuilder();
            query0.Append(" SELECT COUNT(1) as count FROM p700Shop");
            IEnumerable<object> result;
            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            if (urlQueryParameters.id == 0)
            {
                result = await base.DbQueryAsync<object>(query0.ToString(), parameters);
            }
            else
            {
                result = await base.DbQueryAsync<object>(query.ToString(), parameters);
            }
            return result;
        }

        public async Task<IEnumerable<E700Shop>> GetProductSearchAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E700Shop> shops;

            StringBuilder query = new StringBuilder();
            query.Append(" SELECT * FROM p700Shop ");
            query.Append(" WHERE Title LIKE '%"+urlQueryParameters.condition+"%' ");
            query.Append(" ORDER BY id OFFSET " + urlQueryParameters.offset + " ROWS FETCH NEXT " + urlQueryParameters.limit + " ROWS ONLY"); 

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            shops = await DbQueryAsync<E700Shop>(query.ToString(), parameters);

            return shops;
        }
        public async Task<IEnumerable<object>> GetProductCountSearchAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<object> shops;

            StringBuilder query = new StringBuilder();
            query.Append(" SELECT COUNT(1) as count FROM p700Shop WHERE Title LIKE '%"+urlQueryParameters.condition+"%' ");  

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            shops = await DbQueryAsync<object>(query.ToString(), parameters);

            return shops;
        }



    }
}