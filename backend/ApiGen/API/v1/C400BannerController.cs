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
    public class C400BannerController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID400BannerDataAccess _d400BannerDataAccess;

        public C400BannerController(ID400BannerDataAccess d400BannerDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d400BannerDataAccess = d400BannerDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data Banner
            if (what == 400)
            {
                // Call get all data Banner
                IEnumerable<E400Banner> banner = await _d400BannerDataAccess.GetAllAsync();
               
                return JsonConvert.SerializeObject(banner, Formatting.Indented);
            }

            // Insert data to table Banner
            if (what == 401)
            {
                // Auto map request param data to Entity
                var banner = _mapper.Map<E400Banner>(param);

                // Call insert all data to Banner table
                var result = await _d400BannerDataAccess.CreateAsync(banner);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table Banner
            if (what == 402)
            {
                // Auto map request param data to Entity
                var banner = _mapper.Map<E400Banner>(param);
                banner.id = param.id.Value;

                // Call insert all data to Banner table
                var result = await _d400BannerDataAccess.UpdateAsync(banner);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data Banner by Id
            if (what == 403)
            {
                // Get id Banner need delete
                var listid = param.listid.Value;

                // Call delete all data Banner table by list id
                var result = await _d400BannerDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data Banner by Id
            if (what == 404)
            {
                // Get id Banner need delete
                var id = param.id.Value;

                // Call find Banner from table by id
                var result = await _d400BannerDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data Banner Pagination 
            if (what == 405)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from Banner table have pagination
                var result = await _d400BannerDataAccess.GetPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check Banner exists by Id
            if (what == 406)
            {
                // Get id Banner need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }
                // Call check Banner in table
                var result = await _d400BannerDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            if (what == 407)
            {
                // Call get all data Banner
                IEnumerable<E400Banner> banner = await _d400BannerDataAccess.GetAllArragePositionAsync();

                return JsonConvert.SerializeObject(banner, Formatting.Indented);
            }

            return null;
        }
    }
}