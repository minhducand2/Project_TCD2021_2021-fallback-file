using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID1800UserStatusDataAccess
    {
        Task<IEnumerable<E1800UserStatus>> GetAllAsync();
        Task<long> CreateAsync(E1800UserStatus userStatus);
        Task<bool> UpdateAsync(E1800UserStatus userStatus);
        Task<bool> DeleteAsync(object id);
        Task<E1800UserStatus> GetByIdAsync(object id);
        Task<IEnumerable<E1800UserStatus>> GetPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItem(object id);
        Task<bool> ExecuteWithTransactionScope();
        Task<IEnumerable<object>> CustomJoin(); 
    }
}