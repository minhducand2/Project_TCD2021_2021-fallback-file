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
    public class C200RoleController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID200RoleDataAccess _d400RoleDataAccess;

        public C200RoleController(ID200RoleDataAccess d400RoleDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d400RoleDataAccess = d400RoleDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data Role
            if (what == 200)
            {
                // Call get all data Role
                IEnumerable<E200Role> role = await _d400RoleDataAccess.GetAllAsync();
                return JsonConvert.SerializeObject(role, Formatting.Indented);
            }

            // Insert data to table Role
            if (what == 201)
            {
                // Auto map request param data to Entity
                var role = _mapper.Map<E200Role>(param);

                // Call insert all data to Role table
                var result = await _d400RoleDataAccess.CreateAsync(role);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table Role
            if (what == 202)
            {
                // Auto map request param data to Entity
                var role = _mapper.Map<E200Role>(param);
                role.id = param.id.Value;

                // Call insert all data to Role table
                var result = await _d400RoleDataAccess.UpdateAsync(role);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data Role by Id
            if (what == 203)
            {
                // Get id Role need delete
                var listid = param.listid.Value;

                // Call delete all data Role table by list id
                var result = await _d400RoleDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data Role by Id
            if (what == 204)
            {
                // Get id Role need delete
                var id = param.id.Value;

                // Call find Role from table by id
                var result = await _d400RoleDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data Role Pagination 
            if (what == 205)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from Role table have pagination
                var result = await _d400RoleDataAccess.GetRolesPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check Role exists by Id
            if (what == 206)
            {
                // Get id Menu need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }

                // Call check Role in table
                var result = await _d400RoleDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            return null;
        }
    }
}