using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID400BannerDataAccess
    {
        Task<IEnumerable<E400Banner>> GetAllAsync();
        Task<long> CreateAsync(E400Banner banner);
        Task<bool> UpdateAsync(E400Banner banner);
        Task<bool> DeleteAsync(object id);
        Task<E400Banner> GetByIdAsync(object id);
        Task<IEnumerable<E400Banner>> GetPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItem(object id);
        Task<bool> ExecuteWithTransactionScope();
        Task<IEnumerable<object>> CustomJoin();
        Task<IEnumerable<E400Banner>> GetAllArragePositionAsync();
    }
}