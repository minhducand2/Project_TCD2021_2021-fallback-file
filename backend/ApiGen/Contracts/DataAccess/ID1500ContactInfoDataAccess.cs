using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID1500ContactInfoDataAccess
    {
        Task<IEnumerable<E1500ContactInfo>> GetAllAsync();
        Task<long> CreateAsync(E1500ContactInfo contactInfo);
        Task<bool> UpdateAsync(E1500ContactInfo contactInfo);
        Task<bool> DeleteAsync(object id);
        Task<E1500ContactInfo> GetByIdAsync(object id);
        Task<IEnumerable<E1500ContactInfo>> GetPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItem(object id);
        Task<bool> ExecuteWithTransactionScope();
        Task<IEnumerable<object>> CustomJoin(); 
    }
}