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
    public class C2400PaymentTypeController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID2400PaymentTypeDataAccess _d2400PaymentTypeDataAccess;

        public C2400PaymentTypeController(ID2400PaymentTypeDataAccess d2400PaymentTypeDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d2400PaymentTypeDataAccess = d2400PaymentTypeDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data PaymentType
            if (what == 2400)
            {
                // Call get all data PaymentType
                IEnumerable<E2400PaymentType> paymentType = await _d2400PaymentTypeDataAccess.GetAllAsync();
               
                return JsonConvert.SerializeObject(paymentType, Formatting.Indented);
            }

            // Insert data to table PaymentType
            if (what == 2401)
            {
                // Auto map request param data to Entity
                var paymentType = _mapper.Map<E2400PaymentType>(param);

                // Call insert all data to PaymentType table
                var result = await _d2400PaymentTypeDataAccess.CreateAsync(paymentType);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table PaymentType
            if (what == 2402)
            {
                // Auto map request param data to Entity
                var paymentType = _mapper.Map<E2400PaymentType>(param);
                paymentType.id = param.id.Value;

                // Call insert all data to PaymentType table
                var result = await _d2400PaymentTypeDataAccess.UpdateAsync(paymentType);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data PaymentType by Id
            if (what == 2403)
            {
                // Get id PaymentType need delete
                var listid = param.listid.Value;

                // Call delete all data PaymentType table by list id
                var result = await _d2400PaymentTypeDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data PaymentType by Id
            if (what == 2404)
            {
                // Get id PaymentType need delete
                var id = param.id.Value;

                // Call find PaymentType from table by id
                var result = await _d2400PaymentTypeDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data PaymentType Pagination 
            if (what == 2405)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from PaymentType table have pagination
                var result = await _d2400PaymentTypeDataAccess.GetPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check PaymentType exists by Id
            if (what == 2406)
            {
                // Get id PaymentType need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }
                // Call check PaymentType in table
                var result = await _d2400PaymentTypeDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            } 

            return null;
        }
    }
}