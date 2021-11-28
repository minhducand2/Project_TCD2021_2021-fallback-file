using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID2700ProductTypeDataAccess
    {
        Task<IEnumerable<E2700ProductType>> GetAllAsync();
        Task<long> CreateAsync(E2700ProductType productType);
        Task<bool> UpdateAsync(E2700ProductType productType);
        Task<bool> DeleteAsync(object id);
        Task<E2700ProductType> GetByIdAsync(object id);
        Task<IEnumerable<E2700ProductType>> GetPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItem(object id);
        Task<bool> ExecuteWithTransactionScope();
        Task<IEnumerable<object>> CustomJoin(); 
    }
}