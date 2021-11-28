using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID2800OrderShopDataAccess
    {
        Task<IEnumerable<E2800OrderShop>> GetAllAsync();
        Task<long> CreateAsync(E2800OrderShop orderShop);
        Task<bool> UpdateAsync(E2800OrderShop orderShop);
        Task<bool> UpdatePayAsync(ParametersOderShopCart orderShop);
        Task<bool> UpdateTotalPriceAsync(ParametersOderShopCart parameters);
        Task<bool> DeleteAsync(object id);
        Task<E2800OrderShop> GetByIdAsync(object id);
        Task<object> GetByUserCartAsync(object id);
        Task<object> GetByUserManagerProductAsync(object id);
        Task<object> GetByUserOrderReceivedAsync(object id);
        Task<object> GetByUserOrderRefuseAsync(object id);
        Task<bool> CancelOrderAsync(object id);
        Task<IEnumerable<E2800OrderShop>> GetPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItem(object id);
        Task<bool> ExecuteWithTransactionScope();
        Task<IEnumerable<object>> CustomJoin();
        Task<IEnumerable<object>> CustomJoinOrder1Date(ParametersDateTime parameters);
        Task<IEnumerable<object>> CustomJoinOrder2Date(ParametersDateTime parameters);
        Task<IEnumerable<object>> CustomJoinOrder3Date(ParametersDateTime parameters);
        Task<IEnumerable<object>> CustomJoinOrder4Date(ParametersDateTime parameters);
        Task<IEnumerable<object>> CustomJoinOrder5Date(ParametersDateTime parameters);
        Task<IEnumerable<object>> CustomJoinOrderAllDate(ParametersDateTime parameters);
        Task<bool> GetOrderShopWithUser(ParametersOrderShop parameters);
        Task<object> GetByIdUserCountCartAsync(object IdUser);
        Task<bool> GetByOrederShopResetAsync(ParametersOrderShopReset parameters);
    }
}