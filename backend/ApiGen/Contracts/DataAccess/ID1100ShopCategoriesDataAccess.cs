using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID1100ShopCategoriesDataAccess
    {
        Task<IEnumerable<E1100ShopCategories>> GetAllAsync();
        Task<long> CreateAsync(E1100ShopCategories shopCategories);
        Task<bool> UpdateAsync(E1100ShopCategories shopCategories);
        Task<bool> DeleteAsync(object id);
        Task<E1100ShopCategories> GetByIdAsync(object id);
        Task<IEnumerable<E1100ShopCategories>> GetPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItem(object id);
        Task<bool> ExecuteWithTransactionScope();
        Task<IEnumerable<object>> CustomJoin();
        Task<IEnumerable<E1100ShopCategories>> GetAllRamdomLimit16Async();
    }
}