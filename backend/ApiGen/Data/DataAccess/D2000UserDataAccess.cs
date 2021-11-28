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
    public class D2000UserDataAccess : DbFactoryBase, ID2000UserDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D2000UserDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All User Async
        /// </summary>
        /// <returns>IEnumerable<User></returns>
        public async Task<IEnumerable<E2000User>> GetAllAsync()
        {
            return await DbQueryAsync<E2000User>("SELECT id,IdUserStatus,Fullname,Email,Avatar,IdRoleUser,CreatedAt,UpdatedAt,authkey,Phone,Sex,IdCity,IdDistrict,Address,Point FROM p2000User");
        }

        /// <summary>
        /// Create User Async
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E2000User user)
        {
            string sqlQuery = $@"INSERT INTO p2000User(IdUserStatus,Fullname,Email,Password,Avatar,IdRoleUser,CreatedAt,UpdatedAt,authkey,Phone,Sex,IdCity,IdDistrict,Address,Point)
                                 OUTPUT INSERTED.ID
                                 VALUES(@IdUserStatus,@Fullname,@Email,CONVERT(VARCHAR(32), HashBytes('MD5', @Password), 2),@Avatar,@IdRoleUser,@CreatedAt,@UpdatedAt,@authkey,@Phone,@Sex,@IdCity,@IdDistrict,@Address,@Point);
                                  ";

            return await DbQuerySingleAsync<long>(sqlQuery, user);
        }
        public async Task<object> CreateAsyncLogin(E2000User user)
        {
            StringBuilder query = new StringBuilder(); 
            query.Append("INSERT INTO p2000User(IdUserStatus,Fullname,Email,IdRoleUser,authkey,Phone,Sex,IdCity,IdDistrict,Address)");
            query.Append(" OUTPUT INSERTED.ID");
            query.Append(" VALUES(1,'" + user.Fullname + "','" + user.Email + "',1,'" + user.authkey + "','" + user.Phone + "','" + user.Sex + "','" + user.IdCity + "','" + user.IdDistrict + "','" + user.Address + "')");
            var idUser = await DbQuerySingleAsync<IdOrderShop>(query.ToString(), new { user });
          
            if (idUser != null)
            {
                StringBuilder queryUser = new StringBuilder();
                queryUser.Append("SELECT * FROM p2000User  WHERE id = " + idUser.id + "");
                var data = new
                {
                    status = true,
                    data = await DbQuerySingleAsync<object>(queryUser.ToString(), user)
                };
                return data;
            } else
            {
                StringBuilder queryUser1 = new StringBuilder();
                queryUser1.Append("DELETE FROM p2000User WHERE id = " + idUser.id + "");
                var data = new
                {
                    status = false,
                    data = await DbQuerySingleAsync<object>(queryUser1.ToString(), user)
                };
                return data;
            } 
        }

        public async Task<object> CreateAsyncRegistration(E2000User user)
        {
            StringBuilder query = new StringBuilder();
            query.Append("INSERT INTO p2000User(IdUserStatus,Fullname,Email,IdRoleUser,authkey,Phone,Sex,IdCity,IdDistrict,Address,Password)");
            query.Append(" OUTPUT INSERTED.ID");
            query.Append(" VALUES(1,'" + user.Fullname + "','" + user.Email + "',1,'" + user.authkey + "','" + user.Phone + "','" + user.Sex + "','" + user.IdCity + "','" + user.IdDistrict + "','" + user.Address + "',CONVERT(VARCHAR(32), HashBytes('MD5', '" + user.Password + "'), 2))");
            var idUser = await DbQuerySingleAsync<IdOrderShop>(query.ToString(), new { user });

            if (idUser != null)
            {
                StringBuilder queryUser = new StringBuilder();
                queryUser.Append("SELECT * FROM p2000User  WHERE id = " + idUser.id + "");
                var data = new
                {
                    status = true,
                    data = await DbQuerySingleAsync<object>(queryUser.ToString(), user)
                };
                return data;
            }
            else
            {
                StringBuilder queryUser1 = new StringBuilder();
                queryUser1.Append("DELETE FROM p2000User WHERE id = " + idUser.id + "");
                var data = new
                {
                    status = false,
                    data = await DbQuerySingleAsync<object>(queryUser1.ToString(), user)
                };
                return data;
            }
        }


        /// <summary>
        /// Update User Async
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E2000User user)
        {
            string sqlQuery = $@"UPDATE p2000User SET IdUserStatus=@IdUserStatus,Fullname=@Fullname,Email=@Email,Password=CONVERT(VARCHAR(32), HashBytes('MD5', @Password), 2),Avatar=@Avatar,IdRoleUser=@IdRoleUser,CreatedAt=@CreatedAt,UpdatedAt=@UpdatedAt,
                                 authkey = @authkey, Phone= @Phone,Sex= @Sex,IdCity= @IdCity,IdDistrict= @IdDistrict, Address=@Address, Point=@Point
                                WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, user);
        }

        public async Task<bool> UpdatePasswordAsync(E2000User user)
        {
            string sqlQuery = $@"UPDATE p2000User SET Password=@Password
                                WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, user);
        }

        /// <summary>
        /// Delete User Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p2000User
                                WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E2000User> GetByIdAsync(object id)
        {
            string sqlQuery = $@"SELECT * FROM p2000User
                                                    WHERE id = @id";
            return await DbQuerySingleAsync<E2000User>(sqlQuery, new { id });
        }

        public async Task<E2000User> GetByLoginAsync(E2000User parameters)
        { 

            StringBuilder queryUser = new StringBuilder();
            queryUser.Append("SELECT id,IdUserStatus,Fullname,Email,Avatar,IdRoleUser,CreatedAt,UpdatedAt,authkey,Phone,Sex,IdCity,IdDistrict,Address FROM p2000User WHERE Email = '" + parameters.Email+ "' AND Password = CONVERT(VARCHAR(32), HashBytes('MD5', '"+parameters.Password+"'), 2)");

            return await DbQuerySingleAsync<E2000User>(queryUser.ToString(), new { parameters });
        }

        /// <summary>
        /// Get Users Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E2000User>> GetPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E2000User> users;
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM p2000User   " + urlQueryParameters.condition + "   ");
            query.Append("  ORDER BY id OFFSET " + urlQueryParameters.offset + " ROWS FETCH NEXT " + urlQueryParameters.limit + " ROWS ONLY");
             

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            users = await DbQueryAsync<E2000User>(query.ToString(), parameters);

            return users;
        } 

        /// <summary>
        /// Count User item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p2000User " + condition;
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
        /// Get All User Join User
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CustomJoin()
        {
            return await DbQueryAsync<object>("SELECT p2000User.*, Person.* FROM p2000User INNER JOIN Person on p2000User.id = Person.Id");
        }

        public async Task<IEnumerable<object>> CustomJoinUserDate(ParametersDateTime parameters)
        {
        
            string sqlQuery = @"SELECT COUNT(1) as UserPeople FROM p2000User WHERE CreatedAt BETWEEN
                                (CASE WHEN @startDate = '0' AND @endDate = '0' THEN '1753-01-01 00:00:00'
                                WHEN @startDate != '0' AND @endDate = '0' THEN  @startDate1 
                                WHEN @startDate = '0' AND @endDate != '0' THEN  '1753-01-01 00:00:00' 
                                ELSE @startDate1 END) 
                                AND                             
                                (CASE WHEN @startDate = '0' AND @endDate = '0' THEN '9999-12-31 23:59:59' 
                                WHEN @startDate != '0' AND @endDate = '0' THEN '9999-12-31 23:59:59' 
                                WHEN @startDate = '0' AND @endDate != '0' THEN @endDate1
                                ELSE @endDate1 END)";

            return await DbQueryAsync<object>(sqlQuery, parameters);
        }

        public async Task<IEnumerable<E2000User>> GetUserLogin(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E2000User> users;

            StringBuilder query = new StringBuilder();


            query.Append(" SELECT * FROM p2000User  WHERE authkey = '"+urlQueryParameters.condition+"'");

            users = await DbQueryAsync<E2000User>(query.ToString(), new { });

            return users;
        }

        public async Task<object> GetByWithEmailAsync(object Email)
        {
            IEnumerable<E2000User> result;
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT id,IdUserStatus,Fullname,Email,Avatar,IdRoleUser,CreatedAt,UpdatedAt,authkey,Phone,Sex,IdCity,IdDistrict,Address FROM p2000User WHERE Email = '" + Email+"'");

            return result = await DbQueryAsync<E2000User>(query.ToString(), new {  });  
        }

        public async Task<E2000User> GetPointByIdAsync(object id)
        {
            string sqlQuery = $@"SELECT id,Point FROM p2000User
                                                    WHERE id = @id";
            return await DbQuerySingleAsync<E2000User>(sqlQuery, new { id });
        }
    }
}