using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID2100PromotionDataAccess
    {
        Task<IEnumerable<E2100Promotion>> GetAllAsync();
        Task<long> CreateAsync(E2100Promotion promotion);
        Task<bool> UpdateAsync(E2100Promotion promotion);
        Task<bool> DeleteAsync(object id);
        Task<E2100Promotion> GetByIdAsync(object id);
        Task<E2100Promotion> GetByPromotionCodeAsync(object PromotionCode);
        Task<IEnumerable<E2100Promotion>> GetPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItem(object id);
        Task<bool> ExecuteWithTransactionScope();
        Task<IEnumerable<object>> CustomJoin(); 
    }
}