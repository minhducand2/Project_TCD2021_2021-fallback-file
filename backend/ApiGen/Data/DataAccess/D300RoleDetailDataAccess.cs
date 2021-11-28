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
    public class D300RoleDetailDataAccess : DbFactoryBase, ID300RoleDetailDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D300RoleDetailDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All Role Async
        /// </summary>
        /// <returns>IEnumerable<Role></returns>
        public async Task<IEnumerable<E300RoleDetail>> GetAllAsync()
        {
            return await DbQueryAsync<E300RoleDetail>("SELECT * FROM p200RoleDetail");
        }

        /// <summary>
        /// Create Role Async
        /// </summary>
        /// <param name="sinhVien"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E300RoleDetail sinhVien)
        {
            string sqlQuery = $@"INSERT INTO p300RoleDetail(IdRole,IdMenu,Status) 
                                    OUTPUT INSERTED.ID
                                 VALUES(@IdRole,@IdMenu,@Status); ";

            return await DbQuerySingleAsync<long>(sqlQuery, sinhVien);
        }

        /// <summary>
        /// Update Role Async
        /// </summary>
        /// <param name="sinhVien"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E300RoleDetail sinhVien)
        {
            string sqlQuery = $@"UPDATE p300RoleDetail SET IdRole=@IdRole,IdMenu=@IdMenu,Status=@Status
                                 WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, sinhVien);
        }

        /// <summary>
        /// Delete Role Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p200RoleDetail WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { listid });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E300RoleDetail> GetByIdAsync(object id)
        {
            return await DbQuerySingleAsync<E300RoleDetail>("SELECT * FROM p200RoleDetail WHERE ID = @ID", new { id });
        }

        /// <summary>
        /// Get Roles Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E300RoleDetail>> GetRoleDetailsPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E300RoleDetail> roleDetails;
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM p200RoleDetail   " + urlQueryParameters.condition + "   ");
            query.Append("  ORDER BY id OFFSET " + urlQueryParameters.offset + " ROWS FETCH NEXT " + urlQueryParameters.limit + " ROWS ONLY");
 

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };


            roleDetails = await DbQueryAsync<E300RoleDetail>(query.ToString(), parameters);

            return roleDetails;
        }

        /// <summary>
        /// Count number item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) FROM p200RoleDetail " + condition;
            return await DbQueryAsync<object>(sqlQuery, new { condition });
        }

    }
}