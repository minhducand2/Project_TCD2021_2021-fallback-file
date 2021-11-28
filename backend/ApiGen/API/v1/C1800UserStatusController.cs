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
    public class C1800UserStatusController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID1800UserStatusDataAccess _d1800UserStatusDataAccess;

        public C1800UserStatusController(ID1800UserStatusDataAccess d1800UserStatusDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d1800UserStatusDataAccess = d1800UserStatusDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data UserStatus
            if (what == 1800)
            {
                // Call get all data UserStatus
                IEnumerable<E1800UserStatus> userStatus = await _d1800UserStatusDataAccess.GetAllAsync();
               
                return JsonConvert.SerializeObject(userStatus, Formatting.Indented);
            }

            // Insert data to table UserStatus
            if (what == 1801)
            {
                // Auto map request param data to Entity
                var userStatus = _mapper.Map<E1800UserStatus>(param);

                // Call insert all data to UserStatus table
                var result = await _d1800UserStatusDataAccess.CreateAsync(userStatus);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table UserStatus
            if (what == 1802)
            {
                // Auto map request param data to Entity
                var userStatus = _mapper.Map<E1800UserStatus>(param);
                userStatus.id = param.id.Value;

                // Call insert all data to UserStatus table
                var result = await _d1800UserStatusDataAccess.UpdateAsync(userStatus);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data UserStatus by Id
            if (what == 1803)
            {
                // Get id UserStatus need delete
                var listid = param.listid.Value;

                // Call delete all data UserStatus table by list id
                var result = await _d1800UserStatusDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data UserStatus by Id
            if (what == 1804)
            {
                // Get id UserStatus need delete
                var id = param.id.Value;

                // Call find UserStatus from table by id
                var result = await _d1800UserStatusDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data UserStatus Pagination 
            if (what == 1805)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from UserStatus table have pagination
                var result = await _d1800UserStatusDataAccess.GetPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check UserStatus exists by Id
            if (what == 1806)
            {
                // Get id UserStatus need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }
                // Call check UserStatus in table
                var result = await _d1800UserStatusDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            } 

            return null;
        }
    }
}