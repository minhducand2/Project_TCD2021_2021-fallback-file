using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID2600DistrictDataAccess
    {
        Task<IEnumerable<E2600District>> GetAllAsync();
        Task<long> CreateAsync(E2600District district);
        Task<bool> UpdateAsync(E2600District district);
        Task<bool> DeleteAsync(object id);
        Task<E2600District> GetByIdAsync(object id);
        Task<object> GetByIdCityAsync(object id);
        Task<IEnumerable<E2600District>> GetPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItem(object id);
        Task<bool> ExecuteWithTransactionScope();
        Task<IEnumerable<object>> CustomJoin(); 
    }
}