using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID300RoleDetailDataAccess
    {
        Task<IEnumerable<object>> CountNumberItem(object condition);
        Task<long> CreateAsync(E300RoleDetail sinhVien);
        Task<bool> DeleteAsync(object listid);
        Task<IEnumerable<E300RoleDetail>> GetAllAsync();
        Task<E300RoleDetail> GetByIdAsync(object id);
        Task<IEnumerable<E300RoleDetail>> GetRoleDetailsPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<bool> UpdateAsync(E300RoleDetail sinhVien);
    }
}