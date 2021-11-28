using ApiGen.Data;
using ApiGen.Data.DataAccess;
using ApiGen.Data.Entity; 
using AutoMapper; 
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.API.v1
{
    public class C1200MealPlanTypeController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID1200MealPlanTypeDataAccess _d1200MealPlanTypeDataAccess;

        public C1200MealPlanTypeController(ID1200MealPlanTypeDataAccess d1200MealPlanTypeDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d1200MealPlanTypeDataAccess = d1200MealPlanTypeDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data MealPlanType
            if (what == 1200)
            {
                // Call get all data MealPlanType
                IEnumerable<E1200MealPlanType> mealPlanType = await _d1200MealPlanTypeDataAccess.GetAllAsync();
               
                return JsonConvert.SerializeObject(mealPlanType, Formatting.Indented);
            }

            // Insert data to table MealPlanType
            if (what == 1201)
            {
                // Auto map request param data to Entity
                var mealPlanType = _mapper.Map<E1200MealPlanType>(param);

                // Call insert all data to MealPlanType table
                var result = await _d1200MealPlanTypeDataAccess.CreateAsync(mealPlanType);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table MealPlanType
            if (what == 1202)
            {
                // Auto map request param data to Entity
                var mealPlanType = _mapper.Map<E1200MealPlanType>(param);
                mealPlanType.id = param.id.Value;

                // Call insert all data to MealPlanType table
                var result = await _d1200MealPlanTypeDataAccess.UpdateAsync(mealPlanType);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data MealPlanType by Id
            if (what == 1203)
            {
                // Get id MealPlanType need delete
                var listid = param.listid.Value;

                // Call delete all data MealPlanType table by list id
                var result = await _d1200MealPlanTypeDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data MealPlanType by Id
            if (what == 1204)
            {
                // Get id MealPlanType need delete
                var id = param.id.Value;

                // Call find MealPlanType from table by id
                var result = await _d1200MealPlanTypeDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data MealPlanType Pagination 
            if (what == 1205)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from MealPlanType table have pagination
                var result = await _d1200MealPlanTypeDataAccess.GetPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check MealPlanType exists by Id
            if (what == 1206)
            {
                // Get id MealPlanType need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }
                // Call check MealPlanType in table
                var result = await _d1200MealPlanTypeDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            } 

            return null;
        }
    }
}