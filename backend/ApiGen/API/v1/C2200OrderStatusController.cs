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
    public class C2200OrderStatusController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID2200OrderStatusDataAccess _d2200OrderStatusDataAccess;

        public C2200OrderStatusController(ID2200OrderStatusDataAccess d2200OrderStatusDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d2200OrderStatusDataAccess = d2200OrderStatusDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data OrderStatus
            if (what == 2200)
            {
                // Call get all data OrderStatus
                IEnumerable<E2200OrderStatus> orderStatus = await _d2200OrderStatusDataAccess.GetAllAsync();
               
                return JsonConvert.SerializeObject(orderStatus, Formatting.Indented);
            }

            // Insert data to table OrderStatus
            if (what == 2201)
            {
                // Auto map request param data to Entity
                var orderStatus = _mapper.Map<E2200OrderStatus>(param);

                // Call insert all data to OrderStatus table
                var result = await _d2200OrderStatusDataAccess.CreateAsync(orderStatus);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table OrderStatus
            if (what == 2202)
            {
                // Auto map request param data to Entity
                var orderStatus = _mapper.Map<E2200OrderStatus>(param);
                orderStatus.id = param.id.Value;

                // Call insert all data to OrderStatus table
                var result = await _d2200OrderStatusDataAccess.UpdateAsync(orderStatus);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data OrderStatus by Id
            if (what == 2203)
            {
                // Get id OrderStatus need delete
                var listid = param.listid.Value;

                // Call delete all data OrderStatus table by list id
                var result = await _d2200OrderStatusDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data OrderStatus by Id
            if (what == 2204)
            {
                // Get id OrderStatus need delete
                var id = param.id.Value;

                // Call find OrderStatus from table by id
                var result = await _d2200OrderStatusDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data OrderStatus Pagination 
            if (what == 2205)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from OrderStatus table have pagination
                var result = await _d2200OrderStatusDataAccess.GetPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check OrderStatus exists by Id
            if (what == 2206)
            {
                // Get id OrderStatus need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }
                // Call check OrderStatus in table
                var result = await _d2200OrderStatusDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            } 

            return null;
        }
    }
}