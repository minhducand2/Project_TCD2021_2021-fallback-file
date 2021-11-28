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
    public class C1600ContactStatusController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID1600ContactStatusDataAccess _d1600ContactStatusDataAccess;

        public C1600ContactStatusController(ID1600ContactStatusDataAccess d1600ContactStatusDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d1600ContactStatusDataAccess = d1600ContactStatusDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data ContactStatus
            if (what == 1600)
            {
                // Call get all data ContactStatus
                IEnumerable<E1600ContactStatus> contactStatus = await _d1600ContactStatusDataAccess.GetAllAsync();
               
                return JsonConvert.SerializeObject(contactStatus, Formatting.Indented);
            }

            // Insert data to table ContactStatus
            if (what == 1601)
            {
                // Auto map request param data to Entity
                var contactStatus = _mapper.Map<E1600ContactStatus>(param);

                // Call insert all data to ContactStatus table
                var result = await _d1600ContactStatusDataAccess.CreateAsync(contactStatus);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table ContactStatus
            if (what == 1602)
            {
                // Auto map request param data to Entity
                var contactStatus = _mapper.Map<E1600ContactStatus>(param);
                contactStatus.id = param.id.Value;

                // Call insert all data to ContactStatus table
                var result = await _d1600ContactStatusDataAccess.UpdateAsync(contactStatus);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data ContactStatus by Id
            if (what == 1603)
            {
                // Get id ContactStatus need delete
                var listid = param.listid.Value;

                // Call delete all data ContactStatus table by list id
                var result = await _d1600ContactStatusDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data ContactStatus by Id
            if (what == 1604)
            {
                // Get id ContactStatus need delete
                var id = param.id.Value;

                // Call find ContactStatus from table by id
                var result = await _d1600ContactStatusDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data ContactStatus Pagination 
            if (what == 1605)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from ContactStatus table have pagination
                var result = await _d1600ContactStatusDataAccess.GetPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check ContactStatus exists by Id
            if (what == 1606)
            {
                // Get id ContactStatus need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }
                // Call check ContactStatus in table
                var result = await _d1600ContactStatusDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            } 

            return null;
        }
    }
}