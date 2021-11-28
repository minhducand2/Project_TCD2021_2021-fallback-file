using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID900ShopComboDetailDataAccess
    {
        Task<IEnumerable<E900ShopComboDetail>> GetAllAsync();
        Task<long> CreateAsync(E900ShopComboDetail shopComboDetail);
        Task<bool> UpdateAsync(E900ShopComboDetail shopComboDetail);
        Task<bool> DeleteAsync(object id);
        Task<E900ShopComboDetail> GetByIdAsync(object id);
        Task<IEnumerable<E900ShopComboDetail>> GetPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItem(object id);
        Task<bool> ExecuteWithTransactionScope();
        Task<IEnumerable<object>> CustomJoin(); 
    }
}