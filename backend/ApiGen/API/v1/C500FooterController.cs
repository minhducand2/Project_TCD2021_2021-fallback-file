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
    public class C500FooterController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID500FooterDataAccess _d500FooterDataAccess;

        public C500FooterController(ID500FooterDataAccess d500FooterDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d500FooterDataAccess = d500FooterDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data Footer
            if (what == 500)
            {
                // Call get all data Footer
                IEnumerable<E500Footer> footer = await _d500FooterDataAccess.GetAllAsync();
               
                return JsonConvert.SerializeObject(footer, Formatting.Indented);
            }

            // Insert data to table Footer
            if (what == 501)
            {
                // Auto map request param data to Entity
                var footer = _mapper.Map<E500Footer>(param);

                // Call insert all data to Footer table
                var result = await _d500FooterDataAccess.CreateAsync(footer);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table Footer
            if (what == 502)
            {
                // Auto map request param data to Entity
                var footer = _mapper.Map<E500Footer>(param);
                footer.id = param.id.Value;

                // Call insert all data to Footer table
                var result = await _d500FooterDataAccess.UpdateAsync(footer);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data Footer by Id
            if (what == 503)
            {
                // Get id Footer need delete
                var listid = param.listid.Value;

                // Call delete all data Footer table by list id
                var result = await _d500FooterDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data Footer by Id
            if (what == 504)
            {
                // Get id Footer need delete
                var id = param.id.Value;

                // Call find Footer from table by id
                var result = await _d500FooterDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data Footer Pagination 
            if (what == 505)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from Footer table have pagination
                var result = await _d500FooterDataAccess.GetPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check Footer exists by Id
            if (what == 506)
            {
                // Get id Footer need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }
                // Call check Footer in table
                var result = await _d500FooterDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            } 

            return null;
        }
    }
}