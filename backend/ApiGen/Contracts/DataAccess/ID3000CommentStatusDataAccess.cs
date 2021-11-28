using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID3000CommentStatusDataAccess
    {
        Task<IEnumerable<E3000CommentStatus>> GetAllAsync();
        Task<long> CreateAsync(E3000CommentStatus commentStatus);
        Task<bool> UpdateAsync(E3000CommentStatus commentStatus);
        Task<bool> DeleteAsync(object id);
        Task<E3000CommentStatus> GetByIdAsync(object id);
        Task<IEnumerable<E3000CommentStatus>> GetPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItem(object id);
        Task<bool> ExecuteWithTransactionScope();
        Task<IEnumerable<object>> CustomJoin(); 
    }
}