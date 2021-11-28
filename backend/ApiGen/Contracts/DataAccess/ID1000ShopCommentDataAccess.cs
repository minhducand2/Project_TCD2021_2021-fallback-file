using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID1000ShopCommentDataAccess
    {
        Task<IEnumerable<E1000ShopComment>> GetAllAsync();
        Task<long> CreateAsync(E1000ShopComment shopComment);
        Task<bool> UpdateAsync(E1000ShopComment shopComment);
        Task<bool> DeleteAsync(object id);
        Task<E1000ShopComment> GetByIdAsync(object id);
        Task<IEnumerable<E1000ShopComment>> GetPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> GetPaginationParentAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> GetPaginationChildAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItem(object id);
        Task<bool> ExecuteWithTransactionScope();
        Task<IEnumerable<object>> CustomJoin();
        Task<object> GetPaginationShopCommentAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItemComment(object id);
        Task<bool> GetInsertShopCommentAsync(ParametersShopComment queryParam);
    }
}