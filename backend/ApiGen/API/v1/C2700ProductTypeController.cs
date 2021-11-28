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
    public class C2700ProductTypeController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID2700ProductTypeDataAccess _d2700ProductTypeDataAccess;

        public C2700ProductTypeController(ID2700ProductTypeDataAccess d2700ProductTypeDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d2700ProductTypeDataAccess = d2700ProductTypeDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data ProductType
            if (what == 2700)
            {
                // Call get all data ProductType
                IEnumerable<E2700ProductType> productType = await _d2700ProductTypeDataAccess.GetAllAsync();
               
                return JsonConvert.SerializeObject(productType, Formatting.Indented);
            }

            // Insert data to table ProductType
            if (what == 2701)
            {
                // Auto map request param data to Entity
                var productType = _mapper.Map<E2700ProductType>(param);

                // Call insert all data to ProductType table
                var result = await _d2700ProductTypeDataAccess.CreateAsync(productType);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table ProductType
            if (what == 2702)
            {
                // Auto map request param data to Entity
                var productType = _mapper.Map<E2700ProductType>(param);
                productType.id = param.id.Value;

                // Call insert all data to ProductType table
                var result = await _d2700ProductTypeDataAccess.UpdateAsync(productType);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data ProductType by Id
            if (what == 2703)
            {
                // Get id ProductType need delete
                var listid = param.listid.Value;

                // Call delete all data ProductType table by list id
                var result = await _d2700ProductTypeDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data ProductType by Id
            if (what == 2704)
            {
                // Get id ProductType need delete
                var id = param.id.Value;

                // Call find ProductType from table by id
                var result = await _d2700ProductTypeDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data ProductType Pagination 
            if (what == 2705)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from ProductType table have pagination
                var result = await _d2700ProductTypeDataAccess.GetPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check ProductType exists by Id
            if (what == 2706)
            {
                // Get id ProductType need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }
                // Call check ProductType in table
                var result = await _d2700ProductTypeDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            } 

            return null;
        }
    }
}