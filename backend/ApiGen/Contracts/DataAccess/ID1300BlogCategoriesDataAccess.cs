using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID1300BlogCategoriesDataAccess
    {
        Task<IEnumerable<E1300BlogCategories>> GetAllAsync();
        Task<long> CreateAsync(E1300BlogCategories blogCategories);
        Task<bool> UpdateAsync(E1300BlogCategories blogCategories);
        Task<bool> DeleteAsync(object id);
        Task<E1300BlogCategories> GetByIdAsync(object id);
        Task<IEnumerable<E1300BlogCategories>> GetPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItem(object id);
        Task<bool> ExecuteWithTransactionScope();
        Task<IEnumerable<object>> CustomJoin(); 
    }
}