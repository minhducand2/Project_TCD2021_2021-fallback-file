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
    public class D2400PaymentTypeDataAccess : DbFactoryBase, ID2400PaymentTypeDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D2400PaymentTypeDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All PaymentType Async
        /// </summary>
        /// <returns>IEnumerable<PaymentType></returns>
        public async Task<IEnumerable<E2400PaymentType>> GetAllAsync()
        {
            return await DbQueryAsync<E2400PaymentType>("SELECT id,Name FROM p2400PaymentType");
        }

        /// <summary>
        /// Create PaymentType Async
        /// </summary>
        /// <param name="paymentType"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E2400PaymentType paymentType)
        {
            string sqlQuery = $@"INSERT INTO p2400PaymentType(Name)
                                 OUTPUT INSERTED.ID
                                 VALUES(@Name);
                                  ";

            return await DbQuerySingleAsync<long>(sqlQuery, paymentType);
        }

        /// <summary>
        /// Update PaymentType Async
        /// </summary>
        /// <param name="paymentType"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E2400PaymentType paymentType)
        {
            string sqlQuery = $@"UPDATE p2400PaymentType SET Name=@Name
                                 WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, paymentType);
        }

        /// <summary>
        /// Delete PaymentType Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p2400PaymentType
                                WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E2400PaymentType> GetByIdAsync(object id)
        {
            string sqlQuery = $@"SELECT * FROM p2400PaymentType
                                                    WHERE id = @id";
            return await DbQuerySingleAsync<E2400PaymentType>(sqlQuery, new { id });
        }

        /// <summary>
        /// Get PaymentTypes Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E2400PaymentType>> GetPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E2400PaymentType> paymentTypes;
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM p2400PaymentType   " + urlQueryParameters.condition + "   ");
            query.Append("  ORDER BY id OFFSET " + urlQueryParameters.offset + " ROWS FETCH NEXT " + urlQueryParameters.limit + " ROWS ONLY");

             

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            paymentTypes = await DbQueryAsync<E2400PaymentType>(query.ToString(), parameters);

            return paymentTypes;
        } 

        /// <summary>
        /// Count PaymentType item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p2400PaymentType " + condition;
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
        /// Get All PaymentType Join PaymentType
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CustomJoin()
        {
            return await DbQueryAsync<object>("SELECT p2400PaymentType.*, Person.* FROM p2400PaymentType INNER JOIN Person on p2400PaymentType.id = Person.Id");
        }
    }
}