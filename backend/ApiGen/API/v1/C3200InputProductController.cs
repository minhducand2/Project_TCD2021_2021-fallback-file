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
    public class C3200InputProductController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID3200InputProductDataAccess _d3200InputProductDataAccess;

        public C3200InputProductController(ID3200InputProductDataAccess d3200InputProductDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d3200InputProductDataAccess = d3200InputProductDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data InputProduct
            if (what == 3200)
            {
                // Call get all data InputProduct
                IEnumerable<E3200InputProduct> inputProduct = await _d3200InputProductDataAccess.GetAllAsync();
               
                return JsonConvert.SerializeObject(inputProduct, Formatting.Indented);
            }

            // Insert data to table InputProduct
            if (what == 3201)
            {
                // Auto map request param data to Entity
                var inputProduct = _mapper.Map<E3200InputProduct>(param);
                inputProduct.CreatedAt = TypeConverterExtension.ToDateTime(Convert.ToString(param.CreatedAt));
                inputProduct.ExpiryDate = TypeConverterExtension.ToDateTime(Convert.ToString(param.ExpiryDate));

                // Call insert all data to InputProduct table
                var result = await _d3200InputProductDataAccess.CreateAsync(inputProduct);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table InputProduct
            if (what == 3202)
            {
                // Auto map request param data to Entity
                var inputProduct = _mapper.Map<E3200InputProduct>(param);
                inputProduct.id = param.id.Value;
                inputProduct.CreatedAt = TypeConverterExtension.ToDateTime(Convert.ToString(param.CreatedAt));
                inputProduct.ExpiryDate = TypeConverterExtension.ToDateTime(Convert.ToString(param.ExpiryDate));

                // Call insert all data to InputProduct table
                var result = await _d3200InputProductDataAccess.UpdateAsync(inputProduct);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data InputProduct by Id
            if (what == 3203)
            {
                // Get id InputProduct need delete
                var listid = param.listid.Value;

                // Call delete all data InputProduct table by list id
                var result = await _d3200InputProductDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data InputProduct by Id
            if (what == 3204)
            {
                // Get id InputProduct need delete
                var id = param.id.Value;

                // Call find InputProduct from table by id
                var result = await _d3200InputProductDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data InputProduct Pagination 
            if (what == 3205)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from InputProduct table have pagination
                var result = await _d3200InputProductDataAccess.GetPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check InputProduct exists by Id
            if (what == 3206)
            {
                // Get id InputProduct need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }
                // Call check InputProduct in table
                var result = await _d3200InputProductDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            } 

            return null;
        }
    }
}