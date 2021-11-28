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
    public class C3100MyPromotionController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID3100MyPromotionDataAccess _d3100MyPromotionDataAccess;

        public C3100MyPromotionController(ID3100MyPromotionDataAccess d3100MyPromotionDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d3100MyPromotionDataAccess = d3100MyPromotionDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data MyPromotion
            if (what == 3100)
            {
                // Call get all data MyPromotion
                IEnumerable<E3100MyPromotion> myPromotion = await _d3100MyPromotionDataAccess.GetAllAsync();
               
                return JsonConvert.SerializeObject(myPromotion, Formatting.Indented);
            }

            // Insert data to table MyPromotion
            if (what == 3101)
            {
                // Auto map request param data to Entity
                var myPromotion = _mapper.Map<E3100MyPromotion>(param);
                myPromotion.CreatedAt = TypeConverterExtension.ToDateTime(Convert.ToString(param.CreatedAt));

                // Call insert all data to MyPromotion table
                var result = await _d3100MyPromotionDataAccess.CreateAsync(myPromotion);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table MyPromotion
            if (what == 3102)
            {
                // Auto map request param data to Entity
                var myPromotion = _mapper.Map<E3100MyPromotion>(param);
                myPromotion.id = param.id.Value;
                myPromotion.CreatedAt = TypeConverterExtension.ToDateTime(Convert.ToString(param.CreatedAt));
                
                // Call insert all data to MyPromotion table
                var result = await _d3100MyPromotionDataAccess.UpdateAsync(myPromotion);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data MyPromotion by Id
            if (what == 3103)
            {
                // Get id MyPromotion need delete
                var listid = param.listid.Value;

                // Call delete all data MyPromotion table by list id
                var result = await _d3100MyPromotionDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data MyPromotion by Id
            if (what == 3104)
            {
                // Get id MyPromotion need delete
                var id = param.id.Value;

                // Call find MyPromotion from table by id
                var result = await _d3100MyPromotionDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data MyPromotion Pagination 
            if (what == 3105)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from MyPromotion table have pagination
                var result = await _d3100MyPromotionDataAccess.GetPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check MyPromotion exists by Id
            if (what == 3106)
            {
                // Get id MyPromotion need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }
                // Call check MyPromotion in table
                var result = await _d3100MyPromotionDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            } 

            return null;
        }
    }
}