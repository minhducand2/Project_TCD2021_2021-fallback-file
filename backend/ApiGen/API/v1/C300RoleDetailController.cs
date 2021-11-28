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
    public class C300RoleDetailController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID300RoleDetailDataAccess _d400RoleDetailDataAccess;

        public C300RoleDetailController(ID300RoleDetailDataAccess d400RoleDetailDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d400RoleDetailDataAccess = d400RoleDetailDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data RoleDetail
            if (what == 300)
            {
                // Call get all data RoleDetail
                IEnumerable<E300RoleDetail> roleDetail = await _d400RoleDetailDataAccess.GetAllAsync();
                return JsonConvert.SerializeObject(roleDetail, Formatting.Indented);
            }

            // Insert data to table RoleDetail
            if (what == 301)
            {
                // Auto map request param data to Entity
                var roleDetail = _mapper.Map<E300RoleDetail>(param);

                // Call insert all data to RoleDetail table
                var result = await _d400RoleDetailDataAccess.CreateAsync(roleDetail);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table RoleDetail
            if (what == 302)
            {
                // Auto map request param data to Entity
                var roleDetail = _mapper.Map<E300RoleDetail>(param);
                roleDetail.id = param.id.Value;

                // Call insert all data to RoleDetail table
                var result = await _d400RoleDetailDataAccess.UpdateAsync(roleDetail);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data RoleDetail by Id
            if (what == 303)
            {
                // Get id RoleDetail need delete
                var listid = param.listid.Value;

                // Call delete all data RoleDetail table by list id
                var result = await _d400RoleDetailDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data RoleDetail by Id
            if (what == 304)
            {
                // Get id RoleDetail need delete
                var id = param.id.Value;

                // Call find RoleDetail from table by id
                var result = await _d400RoleDetailDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data RoleDetail Pagination 
            if (what == 305)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from RoleDetail table have pagination
                var result = await _d400RoleDetailDataAccess.GetRoleDetailsPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check RoleDetail exists by Id
            if (what == 306)
            {
                // Get id Menu need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }
                // Call check RoleDetail in table
                var result = await _d400RoleDetailDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            return null;
        }
    }
}