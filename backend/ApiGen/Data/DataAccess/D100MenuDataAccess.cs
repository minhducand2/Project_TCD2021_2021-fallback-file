using ApiGen.Data.Entity;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public class D100MenuDataAccess : DbFactoryBase, ID100MenuDataAccess
    {
        private readonly ILogger<dynamic> _logger;

        public D100MenuDataAccess(IConfiguration config, ILogger<dynamic> logger) : base(config)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All Menu Async
        /// </summary>
        /// <returns>IEnumerable<Menu></returns>
        public async Task<IEnumerable<E100Menu>> GetAllAsync()
        {
            return await DbQueryAsync<E100Menu>("SELECT * FROM p100Menu");
        }

        /// <summary>
        /// Create Menu Async
        /// </summary>
        /// <param name="sinhVien"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(E100Menu sinhVien)
        {
            string sqlQuery = $@"INSERT INTO p100Menu(IdParentMenu,IsGroup,Name,Slug,Icon,Position)
                                    OUTPUT INSERTED.ID
                                 VALUES(@IdParentMenu,@IsGroup,@Name,@Slug,@Icon,@Position); ";

            return await DbQuerySingleAsync<long>(sqlQuery, sinhVien);
        }

        /// <summary>
        /// Update Menu Async
        /// </summary>
        /// <param name="sinhVien"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(E100Menu sinhVien)
        {
            string sqlQuery = $@"UPDATE p100Menu 
                                    SET IdParentMenu=@IdParentMenu,
                                    IsGroup=@IsGroup,
                                    Name=@Name,
                                    Slug=@Slug,
                                    Icon=@Icon,
                                    Position=@Position
                                 WHERE id=@id";

            return await DbExecuteAsync<bool>(sqlQuery, sinhVien);
        }

        /// <summary>
        /// Delete Menu Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(object listid)
        {
            string sqlQuery = $@"DELETE FROM p100Menu
                                 WHERE id IN(" + listid + ")";

            return await DbExecuteAsync<bool>(sqlQuery, new { listid });
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E100Menu> GetByIdAsync(object id)
        {
            return await DbQuerySingleAsync<E100Menu>("SELECT * FROM p100Menu WHERE id = @id", new { id });
        }

        /// <summary>
        /// Get Menus Pagination Async
        /// </summary>
        /// <param name="urlQueryParameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<E100Menu>> GetMenusPaginationAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E100Menu> sinhViens;

            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM p100Menu   " + urlQueryParameters.condition + "   ");
            query.Append("  ORDER BY id OFFSET " + urlQueryParameters.offset + " ROWS FETCH NEXT " + urlQueryParameters.limit + " ROWS ONLY");
 

            var parameters = new
            {
                offset = urlQueryParameters.offset,
                limit = urlQueryParameters.limit
            };


            sinhViens = await DbQueryAsync<E100Menu>(query.ToString(), parameters);

            return sinhViens;
        }

        /// <summary>
        /// Count Number Item
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> CountNumberItem(object condition)
        {
            string sqlQuery = "SELECT COUNT(1) FROM p100Menu " + condition;
            return await DbQueryAsync<object>(sqlQuery);
        }

        /// <summary>
        /// Get Data By Group
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public async Task<IEnumerable<E100Menu>> GetDataByGroup()
        {
            string sqlQuery = "SELECT * FROM p100Menu WHERE IsGroup = '1'";
            return await DbQueryAsync<E100Menu>(sqlQuery, new { });
        }

        /// <summary>
        /// Get all data from p100Menu recusive 108
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> GetDataMenuRecusive108()
        {
            string sqlQuery = $@"SELECT  t1.*, 
                                        t2.id as id1, 
                                        t2.IdParentMenu as IdParentMenu1,
                                        t2.IsGroup as IsGroup1,
                                        t2.Name as Name1, 
                                        t2.Slug as Slug1, 
                                        t2.Icon as Icon1, 
                                        t2.Position as Position1
                                FROM p100Menu t1
                                    LEFT JOIN p100Menu t2 ON t1.id = t2.IdParentMenu  
                                WHERE t1.IdParentMenu = 0
                                ORDER BY t1.position, t1.id, t2.position";

            return await DbQueryAsync<object>(sqlQuery);
        }

        /// <summary>
        /// Get all data from p100Menu recusive 109
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> GetDataMenuRecusive109(object idRole)
        {


            //string sqlQuery = $@"SELECT DISTINCT t1.*, 
            //                            t2.id as id1, 
            //                            t2.IdParentMenu as IdParentMenu1,
            //                            t2.IsGroup as IsGroup1,
            //                            t2.Name as Name1, 
            //                            t2.Slug as Slug1, 
            //                            t2.Icon as Icon1, 
            //                            t2.Position as Position1,
            //                            t3.id as IdRoleDetail,
            //                            t4.id as IdRoleDetail1,
            //                            t3.Status,
            //                            t4.Status as Status1
            //                    FROM p100Menu t1 
            //                        LEFT JOIN p100Menu t2 ON t1.id = t2.IdParentMenu
            //                        LEFT JOIN (SELECT * FROM p300RoleDetail WHERE idRole = @idRole) t3
            //                             ON t1.id = t3.IdMenu  
            //                        LEFT JOIN (SELECT * FROM p300RoleDetail WHERE idRole = @idRole) t4
            //                             ON t2.id = t4.IdMenu  
            //                    WHERE t1.IdParentMenu = 0   
            //                        ORDER BY t1.position, t1.id, t2.position";
                            StringBuilder squerySql = new StringBuilder();
                            squerySql.Append("	SELECT DISTINCT t1.*, ");
                            squerySql.Append("    t2.id as id1, ");
                            squerySql.Append("    t2.IdParentMenu as IdParentMenu1, ");
                            squerySql.Append("    t2.IsGroup as IsGroup1, ");
                            squerySql.Append("    t2.Name as Name1, ");
                            squerySql.Append("    t2.Slug as Slug1, ");
                            squerySql.Append("    t2.Icon as Icon1,   ");
                            squerySql.Append("    t2.Position as Position1,  ");
                            squerySql.Append("    t3.id as IdRoleDetail, ");
                            squerySql.Append("    t4.id as IdRoleDetail1, ");
                            squerySql.Append("    t3.Status, ");
                            squerySql.Append("    t4.Status as Status1 ");
                            squerySql.Append("	FROM p100Menu t1 ");
                            squerySql.Append("	LEFT JOIN p100Menu t2 ON t1.id = t2.IdParentMenu  ");
                            squerySql.Append("	LEFT JOIN (SELECT * FROM p300RoleDetail WHERE idRole = " + idRole + ") t3  ");
                            squerySql.Append("    ON t1.id = t3.IdMenu  ");
                            squerySql.Append("	LEFT JOIN (SELECT * FROM p300RoleDetail WHERE idRole = " + idRole + ") t4  ");
                            squerySql.Append("    ON t2.id = t4.IdMenu  ");
                            squerySql.Append("	WHERE t1.IdParentMenu = 0 ");
                            squerySql.Append("	ORDER BY t1.position, t1.id, t2.position ");


            return await DbQueryAsync<object>(squerySql.ToString(), new { idRole });
        }

        /// <summary>
        /// Get all data from p100Menu recusive 110
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<E100Menu>> GetDataMenuRecusive110(string idUser)
        {
            //string sqlQuery = $@"SELECT DISTINCT   t1.*, 
            //                            t2.id as id1, 
            //                            t2.IdParentMenu as IdParentMenu1,
            //                            t2.IsGroup as IsGroup1,
            //                            t2.Name as Name1, 
            //                            t2.Slug as Slug1, 
            //                            t2.Icon as Icon1, 
            //                            t2.Position as Position1,
            //                            t3.id as IdRoleDetail,
            //                            t4.id as IdRoleDetail1,
            //                            t3.Status,
            //                            t4.Status as Status1
            //                    FROM p100Menu t1 
            //                        LEFT JOIN p100Menu t2 ON t1.id = t2.IdParentMenu
            //                        LEFT JOIN (SELECT * FROM p300RoleDetail WHERE idRole = (SELECT IdRole FROM p000account WHERE id=@idUser)) t3
            //                            ON t1.id = t3.IdMenu  
            //                        LEFT JOIN (SELECT * FROM p300RoleDetail WHERE idRole = (SELECT IdRole FROM p000account WHERE id=@idUser)) t4
            //                            ON t2.id = t4.IdMenu  
            //                    WHERE t1.IdParentMenu = 0   
            //                        ORDER BY t1.position, t1.id, t2.position"; 

            StringBuilder squerySql = new StringBuilder();

            squerySql.Append(" SELECT DISTINCT t1.*,");
            squerySql.Append(" t2.id as id1,");
            squerySql.Append(" t2.IdParentMenu as IdParentMenu1,");
            squerySql.Append(" t2.IsGroup as IsGroup1,");
            squerySql.Append(" t2.Name as Name1,");
            squerySql.Append(" t2.Slug as Slug1,");
            squerySql.Append(" t2.Icon as Icon1,");
            squerySql.Append(" t2.Position as Position1,");
            squerySql.Append(" t3.id as IdRoleDetail,");
            squerySql.Append(" t4.id as IdRoleDetail1,");
            squerySql.Append(" t3.Status,");
            squerySql.Append(" t4.Status as Status1");
            squerySql.Append(" FROM p100Menu t1");
            squerySql.Append(" LEFT JOIN p100Menu t2 ON t1.id = t2.IdParentMenu");
            squerySql.Append(" LEFT JOIN (SELECT * FROM p300RoleDetail WHERE idRole = (SELECT IdRole FROM p000account WHERE id=" + idUser + ")) t3");
            squerySql.Append(" ON t1.id = t3.IdMenu");
            squerySql.Append(" LEFT JOIN (SELECT * FROM p300RoleDetail WHERE idRole = (SELECT IdRole FROM p000account WHERE id=" + idUser + ")) t4");
            squerySql.Append(" ON t2.id = t4.IdMenu");
            squerySql.Append(" WHERE t1.IdParentMenu = 0");
            squerySql.Append("	ORDER BY t1.Position, t1.id, t2.Position ");

            return await DbQueryAsync<E100Menu>(squerySql.ToString(), new { });

        }


        /// <summary> 
        /// Get all data from p100Menu recusive 111
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> GetDataMenuRecusive111(object idUser, object url)
        {
            string sqlQuery = $@"SELECT t1.Status 
                                FROM p300RoleDetail t1
                                    INNER JOIN p200Role t2 on t1.IdRole = t2.id
                                    INNER JOIN p000account t3 on t2.id = t3.IdRole
                                    INNER JOIN p100Menu t4 on t1.IdMenu = t4.id
                                WHERE t3.id = @idUser
                                    AND t4.Slug = @url";

            var parameters = new { idUser = idUser, url = url };

            return await DbQueryAsync<object>(sqlQuery, parameters);
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
                    //Do stuff here Insert, Update or Delete
                    Task q1 = dbCon.ExecuteAsync("<Your SQL Query here>");
                    Task q2 = dbCon.ExecuteAsync("<Your SQL Query here>");
                    Task q3 = dbCon.ExecuteAsync("<Your SQL Query here>");

                    await Task.WhenAll(q1, q2, q3);

                    //Commit the Transaction when all query are executed successfully

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    //Rollback the Transaction when any query fails
                    transaction.Rollback();
                    _logger.Log(LogLevel.Error, ex, "Error when trying to execute database operations within a scope.");

                    return false;
                }
            }
            return true;
        }
    }
}