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
    public class D000AccountDataAccess : DbFactoryBase, ID000AccountDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D000AccountDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All Account Async
        /// </summary>
        /// <returns>IEnumerable<Account></returns>
        public async Task<IEnumerable<E000Account>> GetAllAsync()
        {
            return await DbQueryAsync<E000Account>("SELECT * FROM p000account");
        }

        /// <summary>
        /// Create Account Async
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E000Account account)
        {
            string sqlQuery = $@"INSERT INTO p000account(IdRole,name,email,phone,address)
                                OUTPUT INSERTED.ID
                                 VALUES(@IdRole,@name,@email,@phone,@address); ";

            return await DbQuerySingleAsync<long>(sqlQuery, account);
        }

        /// <summary>
        /// Update Account Async
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E000Account account)
        {
            string sqlQuery = $@"UPDATE p000account 
                                    SET name = @name,
                                    IdRole = @IdRole, 
                                    email = @email, 
                                    phone = @phone,
                                    address = @address, 
                                    role = @role, 
                                    created_date = @created_date1
                                 WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, account);
        }

        /// <summary>
        /// Delete Account Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object id)
        {
            string sqlQuery = $@"DELETE FROM p000account WHERE Id IN(" + id + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        /// <summary>
        /// Get Account by gmail Async
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E000Account>> GetAccountByMailAsync(object email)
        {
            return await DbQueryAsync<E000Account>("SELECT email FROM p000account WHERE email = @email", new { email });
        }

        /// <summary>
        /// Get Account by gmail Async
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E000Account>> CheckLogin(E000Account account)
        {
            string sqlQuery = $@"SELECT * FROM p000account 
                WHERE email = @email AND password = @password";
            return await DbQueryAsync<E000Account>(sqlQuery, account);
        }

        public async Task<IEnumerable<E000Account>> CheckEmail(E000Account account)
        {
            string sqlQuery = $@"SELECT * FROM p000account 
                WHERE email = @email";
            return await DbQueryAsync<E000Account>(sqlQuery, account);
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E000Account>> GetByIdAsync(string id)
        {
            StringBuilder squerySql = new StringBuilder();

            squerySql.Append(" SELECT * FROM p000account WHERE id = "+ id +""); 

            return await DbQueryAsync<E000Account>(squerySql.ToString(), new { id });
        }

        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<long> ChangePassword(E000Account account)
        {
            string sqlQuery = $@"UPDATE p000account SET password=@password WHERE id=@id";

            return await DbQuerySingleAsync<long>(sqlQuery, account);
        }

        /// <summary>
        /// Update Info Staff
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<long> UpdateInfoStaff(E000Account account)
        {
            string sqlQuery = $@"UPDATE p000account SET name=@name WHERE id=@id";

            return await DbQuerySingleAsync<long>(sqlQuery, account);
        }

        /// <summary>
        /// Update Avatar
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<long> UpdateAvatar(E000Account account)
        {
            string sqlQuery = $@"UPDATE p000account SET img=@img WHERE id=@id";

            return await DbQuerySingleAsync<long>(sqlQuery, account);
        }

        /// <summary>
        /// Get Accounts Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E000Account>> GetAccountsPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E000Account> accounts; 

            var query = @"SELECT *                                                                              
                            FROM (SELECT id FROM p000account ORDER BY id OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY) T1     
                            INNER JOIN p000account T2 ON T1.id = T2.id                                               
                                " + urlQueryParameters.condition;

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };
            accounts = await DbQueryAsync<E000Account>(query, parameters);

            return accounts;
        }

        /// <summary>
        /// Exist Account Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> ExistAsync(object id)
        {
            return await DbExecuteScalarAsync("SELECT COUNT(1) as CountPage FROM p000account WHERE id = @id", new { id });
        }

        /// <summary>
        /// Count Accounts
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>  
        public async Task<IEnumerable<object>> CountNumberItem()
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p000account";
            return await DbQueryAsync<object>(sqlQuery, new { });
        }
    }
}