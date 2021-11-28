using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID1400BlogDataAccess
    {
        Task<IEnumerable<E1400Blog>> GetAllAsync();
        Task<long> CreateAsync(E1400Blog blog);
        Task<bool> UpdateAsync(E1400Blog blog);
        Task<bool> DeleteAsync(object id);
        Task<E1400Blog> GetByIdAsync(object id);
        Task<IEnumerable<E1400Blog>> GetPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItem(object id);
        Task<bool> ExecuteWithTransactionScope();
        Task<IEnumerable<object>> CustomJoin();
        Task<IEnumerable<object>> CustomJoinBlog();

    }
}