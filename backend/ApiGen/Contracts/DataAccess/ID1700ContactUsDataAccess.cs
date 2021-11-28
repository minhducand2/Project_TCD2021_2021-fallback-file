using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID1700ContactUsDataAccess
    {
        Task<IEnumerable<E1700ContactUs>> GetAllAsync();
        Task<long> CreateAsync(E1700ContactUs contactUs);
        Task<bool> UpdateAsync(E1700ContactUs contactUs);
        Task<bool> DeleteAsync(object id);
        Task<E1700ContactUs> GetByIdAsync(object id);
        Task<IEnumerable<E1700ContactUs>> GetPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItem(object id);
        Task<bool> ExecuteWithTransactionScope();
        Task<IEnumerable<object>> CustomJoin(); 
    }
}