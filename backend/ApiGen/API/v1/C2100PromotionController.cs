using ApiGen.Data;
using ApiGen.Data.DataAccess;
using ApiGen.Data.Entity;
using ApiGen.Infrastructure.Extensions;
using AutoMapper; 
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.API.v1
{
    public class C2100PromotionController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID2100PromotionDataAccess _d2100PromotionDataAccess;

        public C2100PromotionController(ID2100PromotionDataAccess d2100PromotionDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d2100PromotionDataAccess = d2100PromotionDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data Promotion
            if (what == 2100)
            {
                // Call get all data Promotion
                IEnumerable<E2100Promotion> promotion = await _d2100PromotionDataAccess.GetAllAsync();
               
                return JsonConvert.SerializeObject(promotion, Formatting.Indented);
            }

            // Insert data to table Promotion
            if (what == 2101)
            {
                // Auto map request param data to Entity
                var promotion = _mapper.Map<E2100Promotion>(param);
                promotion.StartDate = TypeConverterExtension.ToDateTime(Convert.ToString(param.StartDate));
                promotion.EndDate = TypeConverterExtension.ToDateTime(Convert.ToString(param.EndDate));
                // Call insert all data to Promotion table
                var result = await _d2100PromotionDataAccess.CreateAsync(promotion);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table Promotion
            if (what == 2102)
            {
                // Auto map request param data to Entity
                var promotion = _mapper.Map<E2100Promotion>(param);
                promotion.id = param.id.Value;

                // Call insert all data to Promotion table
                var result = await _d2100PromotionDataAccess.UpdateAsync(promotion);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data Promotion by Id
            if (what == 2103)
            {
                // Get id Promotion need delete
                var listid = param.listid.Value;

                // Call delete all data Promotion table by list id
                var result = await _d2100PromotionDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data Promotion by Id
            if (what == 2104)
            {
                // Get id Promotion need delete
                var id = param.id.Value;

                // Call find Promotion from table by id
                var result = await _d2100PromotionDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data Promotion Pagination 
            if (what == 2105)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from Promotion table have pagination
                var result = await _d2100PromotionDataAccess.GetPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check Promotion exists by Id
            if (what == 2106)
            {
                // Get id Promotion need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }
                // Call check Promotion in table
                var result = await _d2100PromotionDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            if (what == 2107)
            {
                // Get id Promotion need delete
                var PromotionCode = param.PromotionCode.Value;

                // Call find Promotion from table by id
                var result = await _d2100PromotionDataAccess.GetByPromotionCodeAsync(PromotionCode);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            return null;
        }
    }
}