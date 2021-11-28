using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID2300PaymentStatusDataAccess
    {
        Task<IEnumerable<E2300PaymentStatus>> GetAllAsync();
        Task<long> CreateAsync(E2300PaymentStatus paymentStatus);
        Task<bool> UpdateAsync(E2300PaymentStatus paymentStatus);
        Task<bool> DeleteAsync(object id);
        Task<E2300PaymentStatus> GetByIdAsync(object id);
        Task<IEnumerable<E2300PaymentStatus>> GetPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItem(object id);
        Task<bool> ExecuteWithTransactionScope();
        Task<IEnumerable<object>> CustomJoin(); 
    }
}