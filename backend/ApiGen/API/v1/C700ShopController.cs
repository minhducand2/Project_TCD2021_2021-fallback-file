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
    public class C700ShopController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID700ShopDataAccess _d700ShopDataAccess;

        public C700ShopController(ID700ShopDataAccess d700ShopDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d700ShopDataAccess = d700ShopDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data Shop
            if (what == 700)
            {
                // Call get all data Shop
                IEnumerable<E700Shop> shop = await _d700ShopDataAccess.GetAllAsync();
               
                return JsonConvert.SerializeObject(shop, Formatting.Indented);
            }

            // Insert data to table Shop
            if (what == 701)
            {
                // Auto map request param data to Entity
                var shop = _mapper.Map<E700Shop>(param);

                // Call insert all data to Shop table
                var result = await _d700ShopDataAccess.CreateAsync(shop);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table Shop
            if (what == 702)
            {
                // Auto map request param data to Entity
                var shop = _mapper.Map<E700Shop>(param);
                shop.id = param.id.Value;

                // Call insert all data to Shop table
                var result = await _d700ShopDataAccess.UpdateAsync(shop);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data Shop by Id
            if (what == 703)
            {
                // Get id Shop need delete
                var listid = param.listid.Value;

                // Call delete all data Shop table by list id
                var result = await _d700ShopDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data Shop by Id
            if (what == 704)
            {
                // Get id Shop need delete
                var id = param.id.Value;

                // Call find Shop from table by id
                var result = await _d700ShopDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data Shop Pagination 
            if (what == 705)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from Shop table have pagination
                var result = await _d700ShopDataAccess.GetPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check Shop exists by Id
            if (what == 706)
            {
                // Get id Shop need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }
                // Call check Shop in table
                var result = await _d700ShopDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Top Shop LIMIT 0,10   
            if (what == 707)
            {
                // Call TOP Blog all data to User table
                var result = await _d700ShopDataAccess.CustomJoinTopShop();

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // get Product Ramdom Limit 4
            if (what == 708)
            {
                // Call TOP Blog all data to User table
                var result = await _d700ShopDataAccess.GetAllRamdomLimit4Async();

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }
            // get Product Ramdom Limit 4 order by id
            if (what == 709)
            {
                // Call TOP Blog all data to User table
                var result = await _d700ShopDataAccess.GetAllRamdomLimit4AsyncSugest();

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }
            // get Product Ramdom Limit 8 with PricePromotio > 0
            if (what == 710)
            {
                // Call TOP Blog all data to User table
                var result = await _d700ShopDataAccess.GetAllRamdomLimit8AsyncSugest();

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            //Get data list Product 
            if (what == 711)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);
                queryParam.id = unchecked((int)param.id.Value);

                // Call get all data from Shop table have pagination
                var result = await _d700ShopDataAccess.GetProductListAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }
            //Count page list Product
            if (what == 712)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param); 
                queryParam.id = unchecked((int)param.id.Value);

                // Call get all data from Shop table have pagination
                var result = await _d700ShopDataAccess.GetProductListCountAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }
            // Get data search Product
            if (what == 713)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value); 

                // Call get all data from Shop table have pagination
                var result = await _d700ShopDataAccess.GetProductSearchAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Count page search Product
            if (what == 714)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param); 

                // Call get all data from Shop table have pagination
                var result = await _d700ShopDataAccess.GetProductCountSearchAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }



            return null;
        }
    }
}