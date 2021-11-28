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
    public class D1000ShopCommentDataAccess : DbFactoryBase, ID1000ShopCommentDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D1000ShopCommentDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All ShopComment Async
        /// </summary>
        /// <returns>IEnumerable<ShopComment></returns>
        public async Task<IEnumerable<E1000ShopComment>> GetAllAsync()
        {
            return await DbQueryAsync<E1000ShopComment>("SELECT id,IdShop,IdUser,IdCommentStatus,Content,CreatedAt FROM p1000ShopComment");
        }

        /// <summary>
        /// Create ShopComment Async
        /// </summary>
        /// <param name="shopComment"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E1000ShopComment shopComment)
        {
            string sqlQuery = $@"INSERT INTO p1000ShopComment(IdShop,IdUser,IdCommentStatus,Content,CreatedAt,IdTypeComment,IdStaff)
                                    OUTPUT INSERTED.ID
                                 VALUES(@IdShop,@IdUser,@IdCommentStatus,@Content,@CreatedAt,@IdTypeComment,@IdStaff); ";

            return await DbQuerySingleAsync<long>(sqlQuery, shopComment);
        }

        /// <summary>
        /// Update ShopComment Async
        /// </summary>
        /// <param name="shopComment"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E1000ShopComment shopComment)
        {
            string sqlQuery = $@"UPDATE p1000ShopComment SET IdShop=@IdShop,IdUser=@IdUser,IdCommentStatus=@IdCommentStatus,Content=@Content,CreatedAt=@CreatedAt,IdTypeComment=@IdTypeComment,IdStaff=@IdStaff
                                 WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, shopComment);
        }

        /// <summary>
        /// Delete ShopComment Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p1000ShopComment
                                WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E1000ShopComment> GetByIdAsync(object id)
        {
            string sqlQuery = $@"SELECT * FROM p1000ShopComment
                                                    WHERE id = @id";
            return await DbQuerySingleAsync<E1000ShopComment>(sqlQuery, new { id });
        }

        /// <summary>
        /// Get ShopComments Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E1000ShopComment>> GetPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E1000ShopComment> shopComments;

            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM p1000ShopComment " + urlQueryParameters.condition + " ");
            query.Append("  ORDER BY id OFFSET " + urlQueryParameters.offset + " ROWS FETCH NEXT " + urlQueryParameters.limit + " ROWS ONLY");
 
            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            shopComments = await DbQueryAsync<E1000ShopComment>(query.ToString(), parameters);

            return shopComments;
        } 

        /// <summary>
        /// Count ShopComment item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p1000ShopComment " + condition;
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
        /// Get All ShopComment Join ShopComment
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CustomJoin()
        {
            return await DbQueryAsync<object>("SELECT p1000ShopComment.*, Person.* FROM p1000ShopComment INNER JOIN Person on p1000ShopComment.id = Person.Id");
        }

        public async Task<object> GetPaginationShopCommentAsync(UrlQueryParameters urlQueryParameters)
        {
            //IEnumerable<E1000ShopComment> shopComments;

            var query = @"SELECT t1.*, t2.id as id1, t2.IdShop as IdShop1, t2.IdUser as IdUser1, t2.IdCommentStatus as IdCommentStatus1,
                            t2.Content as Content1, t2.CreatedAt as CreatedAt1, t2.IdTypeComment as IdTypeComment1, t2.IdStaff  as IdStaff1                                                                             
                            FROM p1000ShopComment t1     
                            LEFT JOIN p1000ShopComment t2 ON t1.id = t2.IdTypeComment                                               
                                " + urlQueryParameters.condition + " ORDER BY t1.CreatedAt DESC OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY";

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            var shopComments = await DbQueryAsync<object>(query, parameters);

            return shopComments;
        }

        public async Task<IEnumerable<object>> CountNumberItemComment(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p1000ShopComment t1 LEFT JOIN p1000ShopComment t2 ON T1.id = t2.IdTypeComment  " + condition;
            return await DbQueryAsync<object>(sqlQuery, new { condition });
        }

        public async Task<IEnumerable<object>> GetPaginationParentAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<object> shopComments; 

            StringBuilder sqlquery = new StringBuilder();
            sqlquery.Append("SELECT t1.*,t3.Fullname as Fullname,t3.Avatar as AvatarUser, t4.name as NameStaff, t4.img as AvatarStaff FROM p1000ShopComment t1 ");
            sqlquery.Append(" LEFT JOIN p2000user t3 ON t1.IdUser = t3.id ");
            sqlquery.Append(" LEFT JOIN p000account t4 ON  t1.IdStaff = t4.id ");
            sqlquery.Append(" WHERE t1.IdTypeComment = 0 ");
            sqlquery.Append(" AND t1.IdCommentStatus != 3 ");
            sqlquery.Append(" AND t1.IdShop = '"+ urlQueryParameters.condition+"' ");
            sqlquery.Append(" ORDER BY t1.CreatedAt ");
            sqlquery.Append(" OFFSET "+ urlQueryParameters.offset+" ROWS FETCH NEXT "+ urlQueryParameters.limit+" ROWS ONLY ");

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            shopComments = await DbQueryAsync<object>(sqlquery.ToString(), parameters);

            return shopComments;
        }

        public async Task<IEnumerable<object>> GetPaginationChildAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<object> shopComments;

            StringBuilder sqlquery = new StringBuilder();
            sqlquery.Append("SELECT t2.*, t3.Fullname as Fullname,t3.Avatar as AvatarUser, t4.name as NameStaff, t4.img as AvatarStaff  FROM p1000ShopComment t1 ");
            sqlquery.Append(" LEFT JOIN p1000ShopComment t2 ON t1.id = t2.IdTypeComment ");
            sqlquery.Append(" LEFT JOIN p2000user t3 ON t1.IdUser = t3.id ");
            sqlquery.Append(" LEFT JOIN p000account t4 ON  t2.IdStaff = t4.id ");
            sqlquery.Append(" WHERE t1.IdTypeComment = 0 ");
            sqlquery.Append(" AND t1.IdCommentStatus != 3 ");
            sqlquery.Append(" AND t2.IdCommentStatus != 3 ");
            sqlquery.Append(" AND t1.IdShop = '" + urlQueryParameters.condition + "' ");
            sqlquery.Append(" ORDER BY t1.id, t1.CreatedAt, t2.CreatedAt "); 

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            shopComments = await DbQueryAsync<object>(sqlquery.ToString(), parameters);

            return shopComments;
        }

        public async Task<bool> GetInsertShopCommentAsync(ParametersShopComment queryParam)
        {
         
            StringBuilder sqlquery = new StringBuilder();
            sqlquery.Append(" INSERT INTO p1000ShopComment(IdShop,IdUser,IdCommentStatus,Content,IdTypeComment,IdStaff) ");
            sqlquery.Append("  OUTPUT INSERTED.ID ");
            sqlquery.Append("  VALUES('"+queryParam.IdShop+ "','" + queryParam.IdUser + "',1,N'" + queryParam.Content + "','" + queryParam.IdTypeComment + "',0); ");

            return await DbQuerySingleAsync<bool>(sqlquery.ToString(), new { });
        }
    }
}