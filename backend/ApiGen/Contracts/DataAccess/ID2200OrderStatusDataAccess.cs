using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID2200OrderStatusDataAccess
    {
        Task<IEnumerable<E2200OrderStatus>> GetAllAsync();
        Task<long> CreateAsync(E2200OrderStatus orderStatus);
        Task<bool> UpdateAsync(E2200OrderStatus orderStatus);
        Task<bool> DeleteAsync(object id);
        Task<E2200OrderStatus> GetByIdAsync(object id);
        Task<IEnumerable<E2200OrderStatus>> GetPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItem(object id);
        Task<bool> ExecuteWithTransactionScope();
        Task<IEnumerable<object>> CustomJoin(); 
    }
}