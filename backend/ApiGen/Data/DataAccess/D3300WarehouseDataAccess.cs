using ApiGen.Data.Entity;
using ApiGen.DTO.Response;
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
    public class D3300WarehouseDataAccess : DbFactoryBase, ID3300WarehouseDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D3300WarehouseDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All Warehouse Async
        /// </summary>
        /// <returns>IEnumerable<Warehouse></returns>
        public async Task<IEnumerable<E3300Warehouse>> GetAllAsync()
        {
            return await DbQueryAsync<E3300Warehouse>("SELECT id,IdShop,Amount,IdCity FROM p3300Warehouse");
        }

        public async Task<IEnumerable<object>> GetIdShopAmountAsync()
        {
            return await DbQueryAsync<object>("SELECT IdShop,Amount,IdCity FROM p3300Warehouse");
        }

        /// <summary>
        /// Create Warehouse Async
        /// </summary>
        /// <param name="warehouse"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E3300Warehouse warehouse)
        {
            string sqlQuery = $@"INSERT INTO p3300Warehouse(IdShop,Amount,IdCity)
                                 OUTPUT INSERTED.ID
                                 VALUES(@IdShop,@Amount,@IdCity);
                                 ";

            return await DbQuerySingleAsync<long>(sqlQuery, warehouse);
        }

        /// <summary>
        /// Update Warehouse Async
        /// </summary>
        /// <param name="warehouse"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E3300Warehouse warehouse)
        {
            string sqlQuery = $@"UPDATE p3300Warehouse SET IdShop=@IdShop,Amount=@Amount,IdCity=@IdCity
                                 WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, warehouse);
        }

        /// <summary>
        /// Delete Warehouse Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p3300Warehouse
                                WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E3300Warehouse> GetByIdAsync(object id)
        {
            string sqlQuery = $@"SELECT * FROM p3300Warehouse
                                                    WHERE id = @id";
            return await DbQuerySingleAsync<E3300Warehouse>(sqlQuery, new { id });
        }

        /// <summary>
        /// Get Warehouses Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E3300Warehouse>> GetPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E3300Warehouse> warehouses;
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM p3300Warehouse   " + urlQueryParameters.condition + "   ");
            query.Append("  ORDER BY id OFFSET " + urlQueryParameters.offset + " ROWS FETCH NEXT " + urlQueryParameters.limit + " ROWS ONLY");
 

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            warehouses = await DbQueryAsync<E3300Warehouse>(query.ToString(), parameters);

            return warehouses;
        } 

        /// <summary>
        /// Count Warehouse item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p3300Warehouse " + condition;
            return await DbQueryAsync<object>(sqlQuery, new { condition });
        }

        /// <summary>
        /// Execute With Transaction Scope
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ExecuteWithTransactionScope(IEnumerable<R2900AmountProduct> r2900AmountProduct)
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
        /// Get All Warehouse Join Warehouse
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CustomJoin()
        {
            return await DbQueryAsync<object>("SELECT p3300Warehouse.*, Person.* FROM p3300Warehouse INNER JOIN Person on p3300Warehouse.id = Person.Id");
        }

        public async Task<IEnumerable<object>> SelectWithListId(object listid)
        {
            string sqlQuery = "SELECT * FROM p3300Warehouse WHERE IdShop IN ("+ listid+")   ORDER BY IdShop,ExpiryDate";
            return await DbQueryAsync<object>(sqlQuery, new {  });
        }
        
        public async Task<IEnumerable<object>> SelecSumtWithListId(object listid)
        {
            string sqlQuery = "SELECT IdShop,Sum(Amount) as Amount FROM p3300Warehouse WHERE IdShop IN (" + listid + ") GROUP BY IdShop  ORDER BY IdShop";
            return await DbQueryAsync<object>(sqlQuery, new { });
        }

        public async Task<bool> UpdateAmountWithListId(IEnumerable<R2900AmountProduct> r2900AmountProduct)
           
        {
            StringBuilder sqlQuery = new StringBuilder();

            foreach (var item in r2900AmountProduct)
            {
                sqlQuery = new StringBuilder();
                sqlQuery.Append("UPDATE p3300Warehouse SET Amount= Amount - " + item.UserAmount1 + "");
                sqlQuery.Append("  WHERE IdShop='" + item.IdShop1+ "' AND IdCity='" + item.IdCity + "'");

                await DbExecuteAsync<bool>(sqlQuery.ToString(), new { });
            }


            return true;
        }

        public async Task<bool> UpdateAmountWithListId1(IEnumerable<R2900AmountProduct> r2900AmountProduct)

        {
            StringBuilder sqlQuery = new StringBuilder(); 
            foreach (var item in r2900AmountProduct)
            {
                sqlQuery = new StringBuilder();
                sqlQuery.Append("UPDATE p3300Warehouse SET Amount= Amount + " + item.UserAmount1 + "");
                sqlQuery.Append("  WHERE IdShop='" + item.IdShop1 + "' AND IdCity='" + item.IdCity + "'");

                await DbExecuteAsync<bool>(sqlQuery.ToString(), new { });
            }


            return true;
        } 

        public async Task<bool> UpdateAsyncWithIdShop(E3300Warehouse warehouse)
        {
            string sqlQuery = $@"UPDATE p3300Warehouse SET Amount= Amount + @Amount
                                 WHERE IdShop=@IdShop AND IdCity=@IdCity";

            return await DbExecuteAsync<bool>(sqlQuery, warehouse);
        }

        public async Task<E3300Warehouse> GetByIdShopAsync(ParametersInputWarehouse paramInput)
        { 
            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append("SELECT * FROM p3300Warehouse");
            sqlQuery.Append("  WHERE IdShop = '"+ paramInput .IdShop+ "'");
            sqlQuery.Append("  AND IdCity = '" + paramInput.IdCity + "'");

            return await DbQuerySingleAsync<E3300Warehouse>(sqlQuery.ToString(), new { paramInput });
        }


    }
}