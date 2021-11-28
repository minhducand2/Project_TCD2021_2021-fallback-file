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
    public class C1900RoleUserController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID1900RoleUserDataAccess _d1900RoleUserDataAccess;

        public C1900RoleUserController(ID1900RoleUserDataAccess d1900RoleUserDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d1900RoleUserDataAccess = d1900RoleUserDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data RoleUser
            if (what == 1900)
            {
                // Call get all data RoleUser
                IEnumerable<E1900RoleUser> roleUser = await _d1900RoleUserDataAccess.GetAllAsync();
               
                return JsonConvert.SerializeObject(roleUser, Formatting.Indented);
            }

            // Insert data to table RoleUser
            if (what == 1901)
            {
                // Auto map request param data to Entity
                var roleUser = _mapper.Map<E1900RoleUser>(param);

                // Call insert all data to RoleUser table
                var result = await _d1900RoleUserDataAccess.CreateAsync(roleUser);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table RoleUser
            if (what == 1902)
            {
                // Auto map request param data to Entity
                var roleUser = _mapper.Map<E1900RoleUser>(param);
                roleUser.id = param.id.Value;

                // Call insert all data to RoleUser table
                var result = await _d1900RoleUserDataAccess.UpdateAsync(roleUser);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data RoleUser by Id
            if (what == 1903)
            {
                // Get id RoleUser need delete
                var listid = param.listid.Value;

                // Call delete all data RoleUser table by list id
                var result = await _d1900RoleUserDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data RoleUser by Id
            if (what == 1904)
            {
                // Get id RoleUser need delete
                var id = param.id.Value;

                // Call find RoleUser from table by id
                var result = await _d1900RoleUserDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data RoleUser Pagination 
            if (what == 1905)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from RoleUser table have pagination
                var result = await _d1900RoleUserDataAccess.GetPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check RoleUser exists by Id
            if (what == 1906)
            {
                // Get id RoleUser need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }
                // Call check RoleUser in table
                var result = await _d1900RoleUserDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            } 

            return null;
        }
    }
}