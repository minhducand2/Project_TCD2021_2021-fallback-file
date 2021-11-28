using ApiGen.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.Data.DataAccess
{
    public interface ID1200MealPlanTypeDataAccess
    {
        Task<IEnumerable<E1200MealPlanType>> GetAllAsync();
        Task<long> CreateAsync(E1200MealPlanType mealPlanType);
        Task<bool> UpdateAsync(E1200MealPlanType mealPlanType);
        Task<bool> DeleteAsync(object id);
        Task<E1200MealPlanType> GetByIdAsync(object id);
        Task<IEnumerable<E1200MealPlanType>> GetPaginationAsync(UrlQueryParameters urlQueryParameters);
        Task<IEnumerable<object>> CountNumberItem(object id);
        Task<bool> ExecuteWithTransactionScope();
        Task<IEnumerable<object>> CustomJoin(); 
    }
}