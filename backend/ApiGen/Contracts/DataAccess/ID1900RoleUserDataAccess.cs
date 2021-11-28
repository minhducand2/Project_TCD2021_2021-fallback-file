using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID1900RoleUserDataAccess
    {
        Task<IEnumerable<E1900RoleUser>> GetAllAsync();
        Task<long> CreateAsync(E1900RoleUser roleUser);
        Task<bool> UpdateAsync(E1900RoleUser roleUser);
        Task<bool> DeleteAsync(object id);
        Task<E1900RoleUser> GetByIdAsync(object id);
        Task<IEnumerable<E1900RoleUser>> GetPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItem(object id);
        Task<bool> ExecuteWithTransactionScope();
        Task<IEnumerable<object>> CustomJoin(); 
    }
}