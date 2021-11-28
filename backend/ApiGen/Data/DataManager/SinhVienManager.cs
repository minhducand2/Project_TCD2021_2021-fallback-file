using ApiGen.Contracts;
using ApiGen.Data.Entity;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ApiGen.Data.DataManager
{
    public class SinhVienManager : DbFactoryBase, ISinhVienManager
    {
        private readonly ILogger<SinhVienManager> _logger;

        public SinhVienManager(IConfiguration config, ILogger<SinhVienManager> logger) : base(config)
        {
            _logger = logger;
        }

        public async Task<(IEnumerable<E500SinhVien> SinhViens, Pagination Pagination)> GetSinhViensAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<E500SinhVien> sinhViens;
            int recordCount = default;

            //// For PosgreSql
            //   var query = @"SELECT ID, FirstName, LastName, DateOfBirth FROM SinhVien
            //                ORDER BY ID DESC 
            //                Limit @Limit Offset @Offset";


            //// For SqlServer
            var query = @"SELECT ID, FirstName, LastName, DateOfBirth FROM SinhVien
                            ORDER BY ID DESC
                            OFFSET @Limit * (@Offset -1) ROWS
                            FETCH NEXT @Limit ROWS ONLY";

            var param = new DynamicParameters();
            param.Add("Limit", urlQueryParameters.limit);
            param.Add("Offset", urlQueryParameters.offset);

            if (urlQueryParameters.IncludeCount)
            {
                query += " SELECT COUNT(ID) FROM SinhVien";
                var pagedRows = await DbQueryMultipleAsync<E500SinhVien, int>(query, param);

                sinhViens = pagedRows.Data;
                recordCount = pagedRows.RecordCount;
            }
            else
            {
                sinhViens = await DbQueryAsync<E500SinhVien>(query, param);
            }

            var metadata = new Pagination
            {
                PageNumber = urlQueryParameters.limit,
                PageSize = urlQueryParameters.offset,
                TotalRecords = recordCount

            };

            return (sinhViens, metadata);
        }

        public async Task<IEnumerable<E500SinhVien>> GetAllAsync()
        {
            return await DbQueryAsync<E500SinhVien>("SELECT * FROM SinhVien");
        }

        public async Task<IEnumerable<object>> test()
        {
            return await DbQueryAsync<object>("SELECT SinhVien.*, Person.* FROM SinhVien INNER JOIN Person on SinhVien.id = Person.Id");
        }

        public async Task<E500SinhVien> GetByIdAsync(object id)
        {
            return await DbQuerySingleAsync<E500SinhVien>("SELECT * FROM SinhVien WHERE ID = @ID", new { id });
        }

        public async Task<long> CreateAsync(E500SinhVien sinhVien)
        {
            string sqlQuery = $@"INSERT INTO SinhVien (Fullname) 
                                     VALUES (@Fullname);
                                     SELECT LAST_INSERT_ID();";

            return await DbQuerySingleAsync<long>(sqlQuery, sinhVien);
        }
        public async Task<bool> UpdateAsync(E500SinhVien sinhVien)
        {
            string sqlQuery = $@"IF EXISTS (SELECT 1 FROM SinhVien WHERE ID = @ID) 
                                            UPDATE SinhVien SET FirstName = @FirstName, LastName = @LastName, DateOfBirth = @DateOfBirth
                                            WHERE ID = @ID";

            return await DbExecuteAsync<bool>(sqlQuery, sinhVien);
        }
        public async Task<bool> DeleteAsync(object id)
        {
            string sqlQuery = $@"IF EXISTS (SELECT 1 FROM SinhVien WHERE ID = @ID)
                                        DELETE SinhVien WHERE ID = @ID";

            return await DbExecuteAsync<bool>(sqlQuery, new { id });
        }
        public async Task<bool> ExistAsync(object id)
        {
            return await DbExecuteScalarAsync("SELECT COUNT(1) FROM SinhVien WHERE ID = @ID", new { id });
        }

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
