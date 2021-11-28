using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID700ShopDataAccess
    {
        Task<IEnumerable<E700Shop>> GetAllAsync();
        Task<long> CreateAsync(E700Shop shop);
        Task<bool> UpdateAsync(E700Shop shop);
        Task<bool> DeleteAsync(object id);
        Task<E700Shop> GetByIdAsync(object id);
        Task<IEnumerable<E700Shop>> GetPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<E700Shop>> GetProductListAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<E700Shop>> GetProductSearchAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> GetProductListCountAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> GetProductCountSearchAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItem(object id);
        Task<bool> ExecuteWithTransactionScope();
        Task<IEnumerable<object>> CustomJoin();
        Task<IEnumerable<object>> CustomJoinTopShop();
        Task<IEnumerable<E700Shop>> GetAllRamdomLimit4Async();
        Task<IEnumerable<E700Shop>> GetAllRamdomLimit4AsyncSugest();
        Task<IEnumerable<E700Shop>> GetAllRamdomLimit8AsyncSugest();
    }
}