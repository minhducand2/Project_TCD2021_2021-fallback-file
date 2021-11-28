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
    public class C2500CityController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID2500CityDataAccess _d2500CityDataAccess;

        public C2500CityController(ID2500CityDataAccess d2500CityDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d2500CityDataAccess = d2500CityDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data City
            if (what == 2500)
            {
                // Call get all data City
                IEnumerable<E2500City> city = await _d2500CityDataAccess.GetAllAsync();
               
                return JsonConvert.SerializeObject(city, Formatting.Indented);
            }

            // Insert data to table City
            if (what == 2501)
            {
                // Auto map request param data to Entity
                var city = _mapper.Map<E2500City>(param);

                // Call insert all data to City table
                var result = await _d2500CityDataAccess.CreateAsync(city);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table City
            if (what == 2502)
            {
                // Auto map request param data to Entity
                var city = _mapper.Map<E2500City>(param);
                city.id = param.id.Value;

                // Call insert all data to City table
                var result = await _d2500CityDataAccess.UpdateAsync(city);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data City by Id
            if (what == 2503)
            {
                // Get id City need delete
                var listid = param.listid.Value;

                // Call delete all data City table by list id
                var result = await _d2500CityDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data City by Id
            if (what == 2504)
            {
                // Get id City need delete
                var id = param.id.Value;

                // Call find City from table by id
                var result = await _d2500CityDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data City Pagination 
            if (what == 2505)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from City table have pagination
                var result = await _d2500CityDataAccess.GetPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check City exists by Id
            if (what == 2506)
            {
                // Get id City need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }
                // Call check City in table
                var result = await _d2500CityDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            } 

            return null;
        }
    }
}