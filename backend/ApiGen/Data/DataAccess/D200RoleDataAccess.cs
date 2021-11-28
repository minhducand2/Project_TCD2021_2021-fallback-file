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
    public class D200RoleDataAccess : DbFactoryBase, ID200RoleDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D200RoleDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All Role Async
        /// </summary>
        /// <returns>IEnumerable<Role></returns>
        public async Task<IEnumerable<E200Role>> GetAllAsync()
        {
            return await DbQueryAsync<E200Role>("SELECT * FROM p200Role");
        }

        /// <summary>
        /// Create Role Async
        /// </summary>
        /// <param name="sinhVien"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E200Role role)
        {
            string sqlQuery = $@"INSERT INTO p200Role (Name) 
                                    OUTPUT INSERTED.ID
                                     VALUES (@Name);";

            return await DbQuerySingleAsync<long>(sqlQuery, role);
        }

        /// <summary>
        /// Update Role Async
        /// </summary>
        /// <param name="sinhVien"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E200Role sinhVien)
        {
            string sqlQuery = $@"UPDATE p200Role SET Name = @Name
                                            WHERE Id = @Id";

            return await DbExecuteAsync<bool>(sqlQuery, sinhVien);
        }

        /// <summary>
        /// Delete Role Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p200Role WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { listid });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E200Role> GetByIdAsync(object id)
        {
            return await DbQuerySingleAsync<E200Role>("SELECT * FROM p200Role WHERE ID = @ID", new { id });
        }

        /// <summary>
        /// Get Roles Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E200Role>> GetRolesPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E200Role> roles;
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM p200Role   " + urlQueryParameters.condition + "   ");
            query.Append("  ORDER BY id OFFSET " + urlQueryParameters.offset + " ROWS FETCH NEXT " + urlQueryParameters.limit + " ROWS ONLY");
 

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };
            roles = await DbQueryAsync<E200Role>(query.ToString(), parameters);

            return roles;
        }

        /// <summary>
        /// Count number item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p200Role " + condition;
            return await DbQueryAsync<object>(sqlQuery, new { condition });
        }

    }
}