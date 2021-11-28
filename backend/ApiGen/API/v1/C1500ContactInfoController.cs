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
    public class C1500ContactInfoController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID1500ContactInfoDataAccess _d1500ContactInfoDataAccess;

        public C1500ContactInfoController(ID1500ContactInfoDataAccess d1500ContactInfoDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d1500ContactInfoDataAccess = d1500ContactInfoDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data ContactInfo
            if (what == 1500)
            {
                // Call get all data ContactInfo
                IEnumerable<E1500ContactInfo> contactInfo = await _d1500ContactInfoDataAccess.GetAllAsync();
               
                return JsonConvert.SerializeObject(contactInfo, Formatting.Indented);
            }

            // Insert data to table ContactInfo
            if (what == 1501)
            {
                // Auto map request param data to Entity
                var contactInfo = _mapper.Map<E1500ContactInfo>(param);

                // Call insert all data to ContactInfo table
                var result = await _d1500ContactInfoDataAccess.CreateAsync(contactInfo);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table ContactInfo
            if (what == 1502)
            {
                // Auto map request param data to Entity
                var contactInfo = _mapper.Map<E1500ContactInfo>(param);
                contactInfo.id = param.id.Value;

                // Call insert all data to ContactInfo table
                var result = await _d1500ContactInfoDataAccess.UpdateAsync(contactInfo);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data ContactInfo by Id
            if (what == 1503)
            {
                // Get id ContactInfo need delete
                var listid = param.listid.Value;

                // Call delete all data ContactInfo table by list id
                var result = await _d1500ContactInfoDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data ContactInfo by Id
            if (what == 1504)
            {
                // Get id ContactInfo need delete
                var id = param.id.Value;

                // Call find ContactInfo from table by id
                var result = await _d1500ContactInfoDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data ContactInfo Pagination 
            if (what == 1505)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from ContactInfo table have pagination
                var result = await _d1500ContactInfoDataAccess.GetPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check ContactInfo exists by Id
            if (what == 1506)
            {
                // Get id ContactInfo need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }
                // Call check ContactInfo in table
                var result = await _d1500ContactInfoDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            } 

            return null;
        }
    }
}