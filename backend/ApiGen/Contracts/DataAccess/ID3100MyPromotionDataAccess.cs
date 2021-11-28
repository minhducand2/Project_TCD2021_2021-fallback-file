using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID3100MyPromotionDataAccess
    {
        Task<IEnumerable<E3100MyPromotion>> GetAllAsync();
        Task<long> CreateAsync(E3100MyPromotion myPromotion);
        Task<bool> UpdateAsync(E3100MyPromotion myPromotion);
        Task<bool> DeleteAsync(object id);
        Task<E3100MyPromotion> GetByIdAsync(object id);
        Task<IEnumerable<E3100MyPromotion>> GetPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItem(object id);
        Task<bool> ExecuteWithTransactionScope();
        Task<IEnumerable<object>> CustomJoin(); 
    }
}