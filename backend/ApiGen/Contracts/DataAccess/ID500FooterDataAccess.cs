using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID500FooterDataAccess
    {
        Task<IEnumerable<E500Footer>> GetAllAsync();
        Task<long> CreateAsync(E500Footer footer);
        Task<bool> UpdateAsync(E500Footer footer);
        Task<bool> DeleteAsync(object id);
        Task<E500Footer> GetByIdAsync(object id);
        Task<IEnumerable<E500Footer>> GetPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItem(object id);
        Task<bool> ExecuteWithTransactionScope();
        Task<IEnumerable<object>> CustomJoin(); 
    }
}