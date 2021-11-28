using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID600HeaderInfoDataAccess
    {
        Task<IEnumerable<E600HeaderInfo>> GetAllAsync();
        Task<long> CreateAsync(E600HeaderInfo headerInfo);
        Task<bool> UpdateAsync(E600HeaderInfo headerInfo);
        Task<bool> DeleteAsync(object id);
        Task<E600HeaderInfo> GetByIdAsync(object id);
        Task<IEnumerable<E600HeaderInfo>> GetPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItem(object id);
        Task<bool> ExecuteWithTransactionScope();
        Task<IEnumerable<object>> CustomJoin(); 
    }
}