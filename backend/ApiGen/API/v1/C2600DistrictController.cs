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
    public class C2600DistrictController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID2600DistrictDataAccess _d2600DistrictDataAccess;

        public C2600DistrictController(ID2600DistrictDataAccess d2600DistrictDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d2600DistrictDataAccess = d2600DistrictDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data District
            if (what == 2600)
            {
                // Call get all data District
                IEnumerable<E2600District> district = await _d2600DistrictDataAccess.GetAllAsync();
               
                return JsonConvert.SerializeObject(district, Formatting.Indented);
            }

            // Insert data to table District
            if (what == 2601)
            {
                // Auto map request param data to Entity
                var district = _mapper.Map<E2600District>(param);

                // Call insert all data to District table
                var result = await _d2600DistrictDataAccess.CreateAsync(district);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table District
            if (what == 2602)
            {
                // Auto map request param data to Entity
                var district = _mapper.Map<E2600District>(param);
                district.id = param.id.Value;

                // Call insert all data to District table
                var result = await _d2600DistrictDataAccess.UpdateAsync(district);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data District by Id
            if (what == 2603)
            {
                // Get id District need delete
                var listid = param.listid.Value;

                // Call delete all data District table by list id
                var result = await _d2600DistrictDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data District by Id
            if (what == 2604)
            {
                // Get id District need delete
                var id = param.id.Value;

                // Call find District from table by id
                var result = await _d2600DistrictDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data District Pagination 
            if (what == 2605)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from District table have pagination
                var result = await _d2600DistrictDataAccess.GetPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check District exists by Id
            if (what == 2606)
            {
                // Get id District need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }
                // Call check District in table
                var result = await _d2600DistrictDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }
            // Find data District by IdCity
            if (what == 2607)
            {
                // Get id District need delete
                var id = param.id.Value;

                // Call find District from table by id
                var result = await _d2600DistrictDataAccess.GetByIdCityAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }




            return null;
        }
    }
}