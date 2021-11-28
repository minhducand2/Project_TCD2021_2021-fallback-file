using ApiGen.Data.Entity;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public class D1400BlogDataAccess : DbFactoryBase, ID1400BlogDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D1400BlogDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All Blog Async
        /// </summary>
        /// <returns>IEnumerable<Blog></returns>
        public async Task<IEnumerable<E1400Blog>> GetAllAsync()
        {
            return await DbQueryAsync<E1400Blog>("SELECT id,IdBlogCategories,Title,Thumbnail,Description,Content,NumberView,CreatedAt,UpdatedAt FROM p1400Blog");
        }

        /// <summary>
        /// Create Blog Async
        /// </summary>
        /// <param name="blog"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E1400Blog blog)
        {
            string sqlQuery = $@"INSERT INTO p1400Blog(IdBlogCategories,Title,Thumbnail,Description,Content,NumberView,CreatedAt,UpdatedAt)
                                OUTPUT INSERTED.ID
                                 VALUES(@IdBlogCategories,@Title,@Thumbnail,@Description,@Content,@NumberView,@CreatedAt,@UpdatedAt);
                                 ";

            return await DbQuerySingleAsync<long>(sqlQuery, blog);
        }

        /// <summary>
        /// Update Blog Async
        /// </summary>
        /// <param name="blog"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E1400Blog blog)
        {
            string sqlQuery = $@"UPDATE p1400Blog SET IdBlogCategories=@IdBlogCategories,Title=@Title,Thumbnail=@Thumbnail,Description=@Description,Content=@Content,NumberView=@NumberView,CreatedAt=@CreatedAt,UpdatedAt=@UpdatedAt
                                 WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, blog);
        }

        /// <summary>
        /// Delete Blog Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p1400Blog
                                WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E1400Blog> GetByIdAsync(object id)
        {
            string sqlQuery = $@"SELECT * FROM p1400Blog
                                                    WHERE id = @id";
            return await DbQuerySingleAsync<E1400Blog>(sqlQuery, new { id });
        }

        /// <summary>
        /// Get Blogs Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E1400Blog>> GetPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E1400Blog> blogs;

            var query = @"SELECT *                                                                              
                            FROM (SELECT id FROM p1400Blog ORDER BY id DESC OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY) T1     
                            INNER JOIN p1400Blog T2 ON T1.id = T2.id                                               
                                " + urlQueryParameters.condition;

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };

            blogs = await DbQueryAsync<E1400Blog>(query, parameters);

            return blogs;
        } 

        /// <summary>
        /// Count Blog item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) as CountPage FROM p1400Blog " + condition;
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
        /// Get All Blog Join Blog
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CustomJoin()
        {
            return await DbQueryAsync<object>("SELECT p1400Blog.*, Person.* FROM p1400Blog INNER JOIN Person on p1400Blog.id = Person.Id");
        }

        public async Task<IEnumerable<object>> CustomJoinBlog()
        {
            string sqlQuery = @"SELECT id, IdBlogCategories, Title, NumberView FROM p1400blog
	                                ORDER BY NumberView DESC
                                    OFFSET 0 ROWS
								    FETCH NEXT 10 ROWS ONLY";
            return await DbQueryAsync<object>(sqlQuery);
        }
    }
}