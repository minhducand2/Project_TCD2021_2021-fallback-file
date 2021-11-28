using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID200RoleDataAccess
    {
        Task<IEnumerable<object>> CountNumberItem(object condition);
        Task<long> CreateAsync(E200Role sinhVien);
        Task<bool> DeleteAsync(object listid);
        Task<IEnumerable<E200Role>> GetAllAsync();
        Task<E200Role> GetByIdAsync(object id);
        Task<IEnumerable<E200Role>> GetRolesPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<bool> UpdateAsync(E200Role sinhVien);
    }
}