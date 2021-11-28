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
    public class C1300BlogCategoriesController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID1300BlogCategoriesDataAccess _d1300BlogCategoriesDataAccess;

        public C1300BlogCategoriesController(ID1300BlogCategoriesDataAccess d1300BlogCategoriesDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d1300BlogCategoriesDataAccess = d1300BlogCategoriesDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data BlogCategories
            if (what == 1300)
            {
                // Call get all data BlogCategories
                IEnumerable<E1300BlogCategories> blogCategories = await _d1300BlogCategoriesDataAccess.GetAllAsync();
               
                return JsonConvert.SerializeObject(blogCategories, Formatting.Indented);
            }

            // Insert data to table BlogCategories
            if (what == 1301)
            {
                // Auto map request param data to Entity
                var blogCategories = _mapper.Map<E1300BlogCategories>(param);

                // Call insert all data to BlogCategories table
                var result = await _d1300BlogCategoriesDataAccess.CreateAsync(blogCategories);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table BlogCategories
            if (what == 1302)
            {
                // Auto map request param data to Entity
                var blogCategories = _mapper.Map<E1300BlogCategories>(param);
                blogCategories.id = param.id.Value;

                // Call insert all data to BlogCategories table
                var result = await _d1300BlogCategoriesDataAccess.UpdateAsync(blogCategories);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data BlogCategories by Id
            if (what == 1303)
            {
                // Get id BlogCategories need delete
                var listid = param.listid.Value;

                // Call delete all data BlogCategories table by list id
                var result = await _d1300BlogCategoriesDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data BlogCategories by Id
            if (what == 1304)
            {
                // Get id BlogCategories need delete
                var id = param.id.Value;

                // Call find BlogCategories from table by id
                var result = await _d1300BlogCategoriesDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data BlogCategories Pagination 
            if (what == 1305)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from BlogCategories table have pagination
                var result = await _d1300BlogCategoriesDataAccess.GetPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check BlogCategories exists by Id
            if (what == 1306)
            {
                // Get id BlogCategories need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }
                // Call check BlogCategories in table
                var result = await _d1300BlogCategoriesDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            } 

            return null;
        }
    }
}