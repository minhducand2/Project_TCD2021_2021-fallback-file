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
    public class C1700ContactUsController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID1700ContactUsDataAccess _d1700ContactUsDataAccess;

        public C1700ContactUsController(ID1700ContactUsDataAccess d1700ContactUsDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d1700ContactUsDataAccess = d1700ContactUsDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data ContactUs
            if (what == 1700)
            {
                // Call get all data ContactUs
                IEnumerable<E1700ContactUs> contactUs = await _d1700ContactUsDataAccess.GetAllAsync();
               
                return JsonConvert.SerializeObject(contactUs, Formatting.Indented);
            }

            // Insert data to table ContactUs
            if (what == 1701)
            {
                // Auto map request param data to Entity
                var contactUs = _mapper.Map<E1700ContactUs>(param);

                // Call insert all data to ContactUs table
                var result = await _d1700ContactUsDataAccess.CreateAsync(contactUs);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table ContactUs
            if (what == 1702)
            {
                // Auto map request param data to Entity
                var contactUs = _mapper.Map<E1700ContactUs>(param);
                contactUs.id = param.id.Value;

                // Call insert all data to ContactUs table
                var result = await _d1700ContactUsDataAccess.UpdateAsync(contactUs);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data ContactUs by Id
            if (what == 1703)
            {
                // Get id ContactUs need delete
                var listid = param.listid.Value;

                // Call delete all data ContactUs table by list id
                var result = await _d1700ContactUsDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data ContactUs by Id
            if (what == 1704)
            {
                // Get id ContactUs need delete
                var id = param.id.Value;

                // Call find ContactUs from table by id
                var result = await _d1700ContactUsDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data ContactUs Pagination 
            if (what == 1705)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from ContactUs table have pagination
                var result = await _d1700ContactUsDataAccess.GetPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check ContactUs exists by Id
            if (what == 1706)
            {
                // Get id ContactUs need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }
                // Call check ContactUs in table
                var result = await _d1700ContactUsDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            } 

            return null;
        }
    }
}