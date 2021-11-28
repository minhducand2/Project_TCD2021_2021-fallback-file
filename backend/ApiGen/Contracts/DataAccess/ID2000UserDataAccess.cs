using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID2000UserDataAccess
    {
        Task<IEnumerable<E2000User>> GetAllAsync();
        Task<long> CreateAsync(E2000User user);
        Task<object> CreateAsyncLogin(E2000User user);
        Task<object> CreateAsyncRegistration(E2000User user);
        Task<bool> UpdateAsync(E2000User user);
        Task<bool> UpdatePasswordAsync(E2000User user);
        Task<bool> DeleteAsync(object id);
        Task<E2000User> GetByIdAsync(object id);
        Task<E2000User> GetPointByIdAsync(object id);
        Task<E2000User> GetByLoginAsync(E2000User parameters);
        Task<object> GetByWithEmailAsync(object Email);
        Task<IEnumerable<E2000User>> GetPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<E2000User>> GetUserLogin(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItem(object id);
        Task<bool> ExecuteWithTransactionScope();
        Task<IEnumerable<object>> CustomJoin();
        Task<IEnumerable<object>> CustomJoinUserDate(ParametersDateTime parameters);
    }
}