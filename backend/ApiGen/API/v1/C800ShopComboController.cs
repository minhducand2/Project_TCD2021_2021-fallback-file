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
    public class C800ShopComboController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID800ShopComboDataAccess _d800ShopComboDataAccess;

        public C800ShopComboController(ID800ShopComboDataAccess d800ShopComboDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d800ShopComboDataAccess = d800ShopComboDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data ShopCombo
            if (what == 800)
            {
                // Call get all data ShopCombo
                IEnumerable<E800ShopCombo> shopCombo = await _d800ShopComboDataAccess.GetAllAsync();
               
                return JsonConvert.SerializeObject(shopCombo, Formatting.Indented);
            }

            // Insert data to table ShopCombo
            if (what == 801)
            {
                // Auto map request param data to Entity
                var shopCombo = _mapper.Map<E800ShopCombo>(param);

                // Call insert all data to ShopCombo table
                var result = await _d800ShopComboDataAccess.CreateAsync(shopCombo);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table ShopCombo
            if (what == 802)
            {
                // Auto map request param data to Entity
                var shopCombo = _mapper.Map<E800ShopCombo>(param);
                shopCombo.id = param.id.Value;

                // Call insert all data to ShopCombo table
                var result = await _d800ShopComboDataAccess.UpdateAsync(shopCombo);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data ShopCombo by Id
            if (what == 803)
            {
                // Get id ShopCombo need delete
                var listid = param.listid.Value;

                // Call delete all data ShopCombo table by list id
                var result = await _d800ShopComboDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data ShopCombo by Id
            if (what == 804)
            {
                // Get id ShopCombo need delete
                var id = param.id.Value;

                // Call find ShopCombo from table by id
                var result = await _d800ShopComboDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data ShopCombo Pagination 
            if (what == 805)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from ShopCombo table have pagination
                var result = await _d800ShopComboDataAccess.GetPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check ShopCombo exists by Id
            if (what == 806)
            {
                // Get id ShopCombo need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }
                // Call check ShopCombo in table
                var result = await _d800ShopComboDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            } 

            return null;
        }
    }
}