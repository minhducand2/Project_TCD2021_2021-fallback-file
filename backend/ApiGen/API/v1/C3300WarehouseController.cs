using ApiGen.Data;
using ApiGen.Data.DataAccess;
using ApiGen.Data.Entity;
using ApiGen.DTO.Response;
using ApiGen.Infrastructure.Extensions;
using AutoMapper; 
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGen.API.v1
{
    public class C3300WarehouseController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID3300WarehouseDataAccess _d3300WarehouseDataAccess; 

        public C3300WarehouseController(ID3300WarehouseDataAccess d3300WarehouseDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d3300WarehouseDataAccess = d3300WarehouseDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data Warehouse
            if (what == 3300)
            {
                // Call get all data Warehouse
                IEnumerable<E3300Warehouse> warehouse = await _d3300WarehouseDataAccess.GetAllAsync();
               
                return JsonConvert.SerializeObject(warehouse, Formatting.Indented);
            }

            // Insert data to table Warehouse
            if (what == 3301)
            {
                // Auto map request param data to Entity
                var warehouse = _mapper.Map<E3300Warehouse>(param);
                warehouse.ExpiryDate = TypeConverterExtension.ToDateTime(Convert.ToString(param.ExpiryDate));
                // Call insert all data to Warehouse table
                var result = await _d3300WarehouseDataAccess.CreateAsync(warehouse);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table Warehouse
            if (what == 3302)
            {
                // Auto map request param data to Entity
                var warehouse = _mapper.Map<E3300Warehouse>(param);
                warehouse.id = param.id.Value;
                warehouse.ExpiryDate = TypeConverterExtension.ToDateTime(Convert.ToString(param.ExpiryDate));

                // Call insert all data to Warehouse table
                var result = await _d3300WarehouseDataAccess.UpdateAsync(warehouse);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data Warehouse by Id
            if (what == 3303)
            {
                // Get id Warehouse need delete
                var listid = param.listid.Value;

                // Call delete all data Warehouse table by list id
                var result = await _d3300WarehouseDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data Warehouse by Id
            if (what == 3304)
            {
                // Get id Warehouse need delete
                var id = param.id.Value;

                // Call find Warehouse from table by id
                var result = await _d3300WarehouseDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data Warehouse Pagination 
            if (what == 3305)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from Warehouse table have pagination
                var result = await _d3300WarehouseDataAccess.GetPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check Warehouse exists by Id
            if (what == 3306)
            {
                // Get id Warehouse need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }
                // Call check Warehouse in table
                var result = await _d3300WarehouseDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find Product with listid
            if (what == 3307)
            {
                // Get id Warehouse need delete
                var listid = param.listid.Value;

                
                var result = await _d3300WarehouseDataAccess.SelectWithListId(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find Sum Product with listid
            if (what == 3308)
            {
                // Get id Warehouse 
                var listid = param.listid.Value;

                // Call delete all data Warehouse table by list id
                var result = await _d3300WarehouseDataAccess.SelecSumtWithListId(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Product Warehouse+
            if (what == 3309)
            {
                // Auto map request param data to Entity
                IEnumerable<R2900AmountProduct> r2900AmountProduct = _mapper.Map<IEnumerable<R2900AmountProduct>>(param.data); 

                 var result = await _d3300WarehouseDataAccess.UpdateAmountWithListId(r2900AmountProduct); 

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Product Warehouse-
            if (what == 3310)
            {
                // Auto map request param data to Entity
                IEnumerable<R2900AmountProduct> r2900AmountProduct = _mapper.Map<IEnumerable<R2900AmountProduct>>(param.data);  

                var result = await _d3300WarehouseDataAccess.UpdateAmountWithListId1(r2900AmountProduct);  

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update Input Warehouse
            if (what == 3311)
            {
                // Auto map request param data to Entity
                var warehouse = _mapper.Map<E3300Warehouse>(param);
                warehouse.IdShop = param.IdShop.Value;
                warehouse.Amount = param.Amount.Value;
                warehouse.ExpiryDate = TypeConverterExtension.ToDateTime(Convert.ToString(param.ExpiryDate));

                //set param input find warehouse ID
                ParametersInputWarehouse paramInput = _mapper.Map<ParametersInputWarehouse>(param);

                var IdShop = param.IdShop.Value;
                var findIdShop = await _d3300WarehouseDataAccess.GetByIdShopAsync(paramInput);
                object result;
                

                //insert data table warehouse
                if (findIdShop == null) {

                    result = await _d3300WarehouseDataAccess.CreateAsync(warehouse);

                }
                //update data table warehouse with IdShop
                else
                {
                  
                    result = await _d3300WarehouseDataAccess.UpdateAsyncWithIdShop(warehouse);
                } 

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }



            return null;
        }
    }
}