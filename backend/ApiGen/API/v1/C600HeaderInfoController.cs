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
    public class C600HeaderInfoController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID600HeaderInfoDataAccess _d600HeaderInfoDataAccess;

        public C600HeaderInfoController(ID600HeaderInfoDataAccess d600HeaderInfoDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d600HeaderInfoDataAccess = d600HeaderInfoDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data HeaderInfo
            if (what == 600)
            {
                // Call get all data HeaderInfo
                IEnumerable<E600HeaderInfo> headerInfo = await _d600HeaderInfoDataAccess.GetAllAsync();
               
                return JsonConvert.SerializeObject(headerInfo, Formatting.Indented);
            }

            // Insert data to table HeaderInfo
            if (what == 601)
            {
                // Auto map request param data to Entity
                var headerInfo = _mapper.Map<E600HeaderInfo>(param);

                // Call insert all data to HeaderInfo table
                var result = await _d600HeaderInfoDataAccess.CreateAsync(headerInfo);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table HeaderInfo
            if (what == 602)
            {
                // Auto map request param data to Entity
                var headerInfo = _mapper.Map<E600HeaderInfo>(param);
                headerInfo.id = param.id.Value;

                // Call insert all data to HeaderInfo table
                var result = await _d600HeaderInfoDataAccess.UpdateAsync(headerInfo);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data HeaderInfo by Id
            if (what == 603)
            {
                // Get id HeaderInfo need delete
                var listid = param.listid.Value;

                // Call delete all data HeaderInfo table by list id
                var result = await _d600HeaderInfoDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data HeaderInfo by Id
            if (what == 604)
            {
                // Get id HeaderInfo need delete
                var id = param.id.Value;

                // Call find HeaderInfo from table by id
                var result = await _d600HeaderInfoDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data HeaderInfo Pagination 
            if (what == 605)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from HeaderInfo table have pagination
                var result = await _d600HeaderInfoDataAccess.GetPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check HeaderInfo exists by Id
            if (what == 606)
            {
                // Get id HeaderInfo need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }
                // Call check HeaderInfo in table
                var result = await _d600HeaderInfoDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            } 

            return null;
        }
    }
}