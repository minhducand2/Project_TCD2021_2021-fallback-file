using ApiGen.Data.Entity;
using ApiGen.DTO.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID3300WarehouseDataAccess
    {
        Task<IEnumerable<E3300Warehouse>> GetAllAsync();
        Task<IEnumerable<object>> GetIdShopAmountAsync();
        Task<long> CreateAsync(E3300Warehouse warehouse);
        Task<bool> UpdateAsync(E3300Warehouse warehouse);
        Task<bool> DeleteAsync(object id);
        Task<E3300Warehouse> GetByIdAsync(object id);
        Task<E3300Warehouse> GetByIdShopAsync(ParametersInputWarehouse IdShop);
        Task<IEnumerable<E3300Warehouse>> GetPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItem(object id);
        Task<bool> ExecuteWithTransactionScope(IEnumerable<R2900AmountProduct> r2900AmountProduct); 
        Task<IEnumerable<object>> CustomJoin();
        Task<IEnumerable<object>> SelectWithListId( object listid);
        Task<IEnumerable<object>> SelecSumtWithListId(object listid);
        Task<bool> UpdateAmountWithListId(IEnumerable<R2900AmountProduct> r2900AmountProduct);
        Task<bool> UpdateAmountWithListId1(IEnumerable<R2900AmountProduct> r2900AmountProduct);   
        Task<bool> UpdateAsyncWithIdShop(E3300Warehouse warehouse);
    }
}