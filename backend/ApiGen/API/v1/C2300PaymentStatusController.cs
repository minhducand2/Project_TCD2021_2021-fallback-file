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
    public class C2300PaymentStatusController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID2300PaymentStatusDataAccess _d2300PaymentStatusDataAccess;

        public C2300PaymentStatusController(ID2300PaymentStatusDataAccess d2300PaymentStatusDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d2300PaymentStatusDataAccess = d2300PaymentStatusDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data PaymentStatus
            if (what == 2300)
            {
                // Call get all data PaymentStatus
                IEnumerable<E2300PaymentStatus> paymentStatus = await _d2300PaymentStatusDataAccess.GetAllAsync();
               
                return JsonConvert.SerializeObject(paymentStatus, Formatting.Indented);
            }

            // Insert data to table PaymentStatus
            if (what == 2301)
            {
                // Auto map request param data to Entity
                var paymentStatus = _mapper.Map<E2300PaymentStatus>(param);

                // Call insert all data to PaymentStatus table
                var result = await _d2300PaymentStatusDataAccess.CreateAsync(paymentStatus);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table PaymentStatus
            if (what == 2302)
            {
                // Auto map request param data to Entity
                var paymentStatus = _mapper.Map<E2300PaymentStatus>(param);
                paymentStatus.id = param.id.Value;

                // Call insert all data to PaymentStatus table
                var result = await _d2300PaymentStatusDataAccess.UpdateAsync(paymentStatus);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data PaymentStatus by Id
            if (what == 2303)
            {
                // Get id PaymentStatus need delete
                var listid = param.listid.Value;

                // Call delete all data PaymentStatus table by list id
                var result = await _d2300PaymentStatusDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data PaymentStatus by Id
            if (what == 2304)
            {
                // Get id PaymentStatus need delete
                var id = param.id.Value;

                // Call find PaymentStatus from table by id
                var result = await _d2300PaymentStatusDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data PaymentStatus Pagination 
            if (what == 2305)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from PaymentStatus table have pagination
                var result = await _d2300PaymentStatusDataAccess.GetPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check PaymentStatus exists by Id
            if (what == 2306)
            {
                // Get id PaymentStatus need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }
                // Call check PaymentStatus in table
                var result = await _d2300PaymentStatusDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            } 

            return null;
        }
    }
}