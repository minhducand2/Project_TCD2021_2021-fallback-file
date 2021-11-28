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
    public class C900ShopComboDetailController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID900ShopComboDetailDataAccess _d900ShopComboDetailDataAccess;

        public C900ShopComboDetailController(ID900ShopComboDetailDataAccess d900ShopComboDetailDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d900ShopComboDetailDataAccess = d900ShopComboDetailDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data ShopComboDetail
            if (what == 900)
            {
                // Call get all data ShopComboDetail
                IEnumerable<E900ShopComboDetail> shopComboDetail = await _d900ShopComboDetailDataAccess.GetAllAsync();
               
                return JsonConvert.SerializeObject(shopComboDetail, Formatting.Indented);
            }

            // Insert data to table ShopComboDetail
            if (what == 901)
            {
                // Auto map request param data to Entity
                var shopComboDetail = _mapper.Map<E900ShopComboDetail>(param);

                // Call insert all data to ShopComboDetail table
                var result = await _d900ShopComboDetailDataAccess.CreateAsync(shopComboDetail);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table ShopComboDetail
            if (what == 902)
            {
                // Auto map request param data to Entity
                var shopComboDetail = _mapper.Map<E900ShopComboDetail>(param);
                shopComboDetail.id = param.id.Value;

                // Call insert all data to ShopComboDetail table
                var result = await _d900ShopComboDetailDataAccess.UpdateAsync(shopComboDetail);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data ShopComboDetail by Id
            if (what == 903)
            {
                // Get id ShopComboDetail need delete
                var listid = param.listid.Value;

                // Call delete all data ShopComboDetail table by list id
                var result = await _d900ShopComboDetailDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data ShopComboDetail by Id
            if (what == 904)
            {
                // Get id ShopComboDetail need delete
                var id = param.id.Value;

                // Call find ShopComboDetail from table by id
                var result = await _d900ShopComboDetailDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data ShopComboDetail Pagination 
            if (what == 905)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from ShopComboDetail table have pagination
                var result = await _d900ShopComboDetailDataAccess.GetPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check ShopComboDetail exists by Id
            if (what == 906)
            {
                // Get id ShopComboDetail need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }
                // Call check ShopComboDetail in table
                var result = await _d900ShopComboDetailDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            } 

            return null;
        }
    }
}