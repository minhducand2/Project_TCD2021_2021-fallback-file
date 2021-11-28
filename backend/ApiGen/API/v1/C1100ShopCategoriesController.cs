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
    public class C1100ShopCategoriesController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID1100ShopCategoriesDataAccess _d1100ShopCategoriesDataAccess;

        public C1100ShopCategoriesController(ID1100ShopCategoriesDataAccess d1100ShopCategoriesDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d1100ShopCategoriesDataAccess = d1100ShopCategoriesDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data ShopCategories
            if (what == 1100)
            {
                // Call get all data ShopCategories
                IEnumerable<E1100ShopCategories> shopCategories = await _d1100ShopCategoriesDataAccess.GetAllAsync();
               
                return JsonConvert.SerializeObject(shopCategories, Formatting.Indented);
            }

            // Insert data to table ShopCategories
            if (what == 1101)
            {
                // Auto map request param data to Entity
                var shopCategories = _mapper.Map<E1100ShopCategories>(param);

                // Call insert all data to ShopCategories table
                var result = await _d1100ShopCategoriesDataAccess.CreateAsync(shopCategories);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table ShopCategories
            if (what == 1102)
            {
                // Auto map request param data to Entity
                var shopCategories = _mapper.Map<E1100ShopCategories>(param);
                shopCategories.id = param.id.Value;

                // Call insert all data to ShopCategories table
                var result = await _d1100ShopCategoriesDataAccess.UpdateAsync(shopCategories);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data ShopCategories by Id
            if (what == 1103)
            {
                // Get id ShopCategories need delete
                var listid = param.listid.Value;

                // Call delete all data ShopCategories table by list id
                var result = await _d1100ShopCategoriesDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data ShopCategories by Id
            if (what == 1104)
            {
                // Get id ShopCategories need delete
                var id = param.id.Value;

                // Call find ShopCategories from table by id
                var result = await _d1100ShopCategoriesDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data ShopCategories Pagination 
            if (what == 1105)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from ShopCategories table have pagination
                var result = await _d1100ShopCategoriesDataAccess.GetPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check ShopCategories exists by Id
            if (what == 1106)
            {
                // Get id ShopCategories need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }
                // Call check ShopCategories in table
                var result = await _d1100ShopCategoriesDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get all data ShopCategories Limit 16 Ramdom
            if (what == 1107)
            {
                // Call get all data ShopCategories
                IEnumerable<E1100ShopCategories> shopCategories = await _d1100ShopCategoriesDataAccess.GetAllRamdomLimit16Async();

                return JsonConvert.SerializeObject(shopCategories, Formatting.Indented);
            }

            return null;
        }
    }
}