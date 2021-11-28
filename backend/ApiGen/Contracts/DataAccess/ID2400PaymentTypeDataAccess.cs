using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID2400PaymentTypeDataAccess
    {
        Task<IEnumerable<E2400PaymentType>> GetAllAsync();
        Task<long> CreateAsync(E2400PaymentType paymentType);
        Task<bool> UpdateAsync(E2400PaymentType paymentType);
        Task<bool> DeleteAsync(object id);
        Task<E2400PaymentType> GetByIdAsync(object id);
        Task<IEnumerable<E2400PaymentType>> GetPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItem(object id);
        Task<bool> ExecuteWithTransactionScope();
        Task<IEnumerable<object>> CustomJoin(); 
    }
}