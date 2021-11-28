using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID3200InputProductDataAccess
    {
        Task<IEnumerable<E3200InputProduct>> GetAllAsync();
        Task<long> CreateAsync(E3200InputProduct inputProduct);
        Task<bool> UpdateAsync(E3200InputProduct inputProduct);
        Task<bool> DeleteAsync(object id);
        Task<E3200InputProduct> GetByIdAsync(object id);
        Task<IEnumerable<E3200InputProduct>> GetPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItem(object id);
        Task<bool> ExecuteWithTransactionScope();
        Task<IEnumerable<object>> CustomJoin(); 
    }
}