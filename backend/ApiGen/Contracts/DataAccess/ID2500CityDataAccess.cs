using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID2500CityDataAccess
    {
        Task<IEnumerable<E2500City>> GetAllAsync();
        Task<long> CreateAsync(E2500City city);
        Task<bool> UpdateAsync(E2500City city);
        Task<bool> DeleteAsync(object id);
        Task<E2500City> GetByIdAsync(object id);
        Task<IEnumerable<E2500City>> GetPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItem(object id);
        Task<bool> ExecuteWithTransactionScope();
        Task<IEnumerable<object>> CustomJoin(); 
    }
}