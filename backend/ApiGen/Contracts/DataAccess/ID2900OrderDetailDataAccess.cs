using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID2900OrderDetailDataAccess
    {
        Task<IEnumerable<E2900OrderDetail>> GetAllAsync();
        Task<long> CreateAsync(E2900OrderDetail orderDetail);
        Task<long> CreateOrderRessetAsync(object orderDetail);
        Task<IEnumerable<object>> SelectWithIdOderShop(ParametersCheckAmount paramInput);
        Task<IEnumerable<object>> SelectWithIdOderShopReset(object id);
        Task<bool> UpdateAsync(E2900OrderDetail orderDetail);
        Task<bool> DeleteAsync(object id);
        Task<bool> UpdateAmountMinusAsync(object id);
        Task<bool> UpdateAmountPlusAsync(object id);
        Task<object> GetByUserManagerProducDetailtAsync(object id);
        Task<E2900OrderDetail> GetByIdAsync(object id);
        Task<IEnumerable<E2900OrderDetail>> GetPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItem(object id);
        Task<bool> ExecuteWithTransactionScope();
        Task<IEnumerable<object>> CustomJoin();
        Task<IEnumerable<object>> CustomJoinAmountDate(ParametersDateTime parameters);
        Task<IEnumerable<object>> CustomJoinMoneyDate(ParametersDateTime parameters);
        Task<IEnumerable<object>> CustomJoinMoneyDay(ParametersDateTime parameters);

        Task<bool> RevierStarOrder(ParametersReviewStar parameters);
    }
}