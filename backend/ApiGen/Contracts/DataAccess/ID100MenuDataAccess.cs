using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID100MenuDataAccess
    {
        Task<IEnumerable<object>> CountNumberItem(object condition);
        Task<long> CreateAsync(E100Menu sinhVien);
        Task<bool> DeleteAsync(object listid);
        Task<bool> ExecuteWithTransactionScope();
        Task<IEnumerable<E100Menu>> GetAllAsync();
        Task<E100Menu> GetByIdAsync(object id);
        Task<IEnumerable<E100Menu>> GetDataByGroup();
        Task<IEnumerable<object>> GetDataMenuRecusive108();
        Task<IEnumerable<object>> GetDataMenuRecusive109(object idRole);
        Task<IEnumerable<E100Menu>> GetDataMenuRecusive110(string idUser);
        Task<IEnumerable<object>> GetDataMenuRecusive111(object idUser, object url);
        Task<IEnumerable<E100Menu>> GetMenusPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<bool> UpdateAsync(E100Menu sinhVien);
    }
}