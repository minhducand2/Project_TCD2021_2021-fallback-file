using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID1600ContactStatusDataAccess
    {
        Task<IEnumerable<E1600ContactStatus>> GetAllAsync();
        Task<long> CreateAsync(E1600ContactStatus contactStatus);
        Task<bool> UpdateAsync(E1600ContactStatus contactStatus);
        Task<bool> DeleteAsync(object id);
        Task<E1600ContactStatus> GetByIdAsync(object id);
        Task<IEnumerable<E1600ContactStatus>> GetPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItem(object id);
        Task<bool> ExecuteWithTransactionScope();
        Task<IEnumerable<object>> CustomJoin(); 
    }
}