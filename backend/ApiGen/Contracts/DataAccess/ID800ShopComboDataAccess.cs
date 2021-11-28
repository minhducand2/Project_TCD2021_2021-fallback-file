using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID800ShopComboDataAccess
    {
        Task<IEnumerable<E800ShopCombo>> GetAllAsync();
        Task<long> CreateAsync(E800ShopCombo shopCombo);
        Task<bool> UpdateAsync(E800ShopCombo shopCombo);
        Task<bool> DeleteAsync(object id);
        Task<E800ShopCombo> GetByIdAsync(object id);
        Task<IEnumerable<E800ShopCombo>> GetPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItem(object id);
        Task<bool> ExecuteWithTransactionScope();
        Task<IEnumerable<object>> CustomJoin(); 
    }
}