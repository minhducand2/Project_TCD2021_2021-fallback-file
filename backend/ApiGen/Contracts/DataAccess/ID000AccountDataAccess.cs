using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID000AccountDataAccess
    {
        Task<long> ChangePassword(E000Account account);
        Task<IEnumerable<object>> CountNumberItem();
        Task<long> CreateAsync(E000Account account);
        Task<bool> DeleteAsync(object id);
        Task<bool> ExistAsync(object id);
        Task<IEnumerable<E000Account>> GetAccountsPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<E000Account>> GetAllAsync();
        Task<IEnumerable<E000Account>> GetByIdAsync(string id);
        Task<IEnumerable<E000Account>> GetAccountByMailAsync(object email);
        Task<IEnumerable<E000Account>> CheckLogin(E000Account account);
        Task<IEnumerable<E000Account>> CheckEmail(E000Account account);
        Task<bool> UpdateAsync(E000Account account);
        Task<long> UpdateAvatar(E000Account account);
        Task<long> UpdateInfoStaff(E000Account account);
    }
}