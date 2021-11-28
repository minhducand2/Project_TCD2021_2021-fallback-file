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
using System.Linq;
using System.Threading.Tasks;

namespace ApiGen.API.v1
{
    
    public class C2900OrderDetailController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID2900OrderDetailDataAccess _d2900OrderDetailDataAccess;
        private ID3300WarehouseDataAccess _d3300WarehouseDataAccess;

        public C2900OrderDetailController(ID2900OrderDetailDataAccess d2900OrderDetailDataAccess, ID3300WarehouseDataAccess d3300WarehouseDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d2900OrderDetailDataAccess = d2900OrderDetailDataAccess;
            _d3300WarehouseDataAccess = d3300WarehouseDataAccess;
            _mapper = mapper;
            _logger = logger;
        } 

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data OrderDetail
            if (what == 2900)
            {
                // Call get all data OrderDetail
                IEnumerable<E2900OrderDetail> orderDetail = await _d2900OrderDetailDataAccess.GetAllAsync();
               
                return JsonConvert.SerializeObject(orderDetail, Formatting.Indented);
            }

            // Insert data to table OrderDetail
            if (what == 2901)
            {
                // Auto map request param data to Entity
                var orderDetail = _mapper.Map<E2900OrderDetail>(param);
                orderDetail.CreatedAt = TypeConverterExtension.ToDateTime(Convert.ToString(param.CreatedAt));
                orderDetail.UpdatedAt = TypeConverterExtension.ToDateTime(Convert.ToString(param.UpdatedAt));

                // Call insert all data to OrderDetail table
                var result = await _d2900OrderDetailDataAccess.CreateAsync(orderDetail);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table OrderDetail
            if (what == 2902)
            {
                // Auto map request param data to Entity
                var orderDetail = _mapper.Map<E2900OrderDetail>(param);
                orderDetail.id = param.id.Value;
                orderDetail.CreatedAt = TypeConverterExtension.ToDateTime(Convert.ToString(param.CreatedAt));
                orderDetail.UpdatedAt = TypeConverterExtension.ToDateTime(Convert.ToString(param.UpdatedAt));

                // Call insert all data to OrderDetail table
                var result = await _d2900OrderDetailDataAccess.UpdateAsync(orderDetail);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data OrderDetail by Id
            if (what == 2903)
            {
                // Get id OrderDetail need delete
                var listid = param.listid.Value;

                // Call delete all data OrderDetail table by list id
                var result = await _d2900OrderDetailDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data OrderDetail by Id
            if (what == 2904)
            {
                // Get id OrderDetail need delete
                var id = param.id.Value;

                // Call find OrderDetail from table by id
                var result = await _d2900OrderDetailDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data OrderDetail Pagination 
            if (what == 2905)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from OrderDetail table have pagination
                var result = await _d2900OrderDetailDataAccess.GetPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check OrderDetail exists by Id
            if (what == 2906)
            {
                // Get id OrderDetail need check
                var Condition = "";
                if (param.condition != null)
                {
                    Condition = param.condition;
                }
                // Call check OrderDetail in table
                var result = await _d2900OrderDetailDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }
            //Amout product Date
            if (what == 2907)
            {
                // Auto map request param data to Entity         
                ParametersDateTime parameters = _mapper.Map<ParametersDateTime>(param);
                parameters.startDate = param.startDate;
                parameters.startDate1 = param.startDate + " 00:00:00";
                parameters.endDate = param.endDate;
                parameters.endDate1 = param.endDate + " 23:59:59";


                // Call insert all data to User table
                var result = await _d2900OrderDetailDataAccess.CustomJoinAmountDate(parameters);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            //Total Money product Date
            if (what == 2908)
            {
                // Auto map request param data to Entity         
                ParametersDateTime parameters = _mapper.Map<ParametersDateTime>(param);
                parameters.startDate = param.startDate;
                parameters.startDate1 = param.startDate + " 00:00:00";
                parameters.endDate = param.endDate;
                parameters.endDate1 = param.endDate + " 23:59:59";

                var CostAll = 0;
                var RevenueAll = 0; 
               
                // Call insert all data to User table
                var result = await _d2900OrderDetailDataAccess.CustomJoinMoneyDate(parameters);
                IEnumerable<R2900OrderDetailResponse> r2900OrderDetailResponses = _mapper.Map<IEnumerable<R2900OrderDetailResponse>>(result);

                // For Total Money
                foreach(R2900OrderDetailResponse item in r2900OrderDetailResponses)
                {
                    CostAll += item.Amount * item.PriceOrigin;
                    RevenueAll += item.TotalPrice; 
                }

                var totalMoney = new { 
                     totalCostAll = CostAll,
                     totalRevenueAll = RevenueAll
                }; 

                return JsonConvert.SerializeObject(totalMoney, Formatting.Indented);
            }

            //Total Money product Day
            if (what == 2909)
            {
                // Auto map request param data to Entity
                ParametersDateTime parameters = _mapper.Map<ParametersDateTime>(param);
                
                parameters.startDate = param.nowDay + " 00:00:00"; 
                parameters.endDate = param.nowDay + " 23:59:59";
              

                var CostAll = 0;
                var RevenueAll = 0;

                // Call insert all data to User table
                var result = await _d2900OrderDetailDataAccess.CustomJoinMoneyDay(parameters);
                IEnumerable<R2900OrderDetailResponse> r2900OrderDetailResponses = _mapper.Map<IEnumerable<R2900OrderDetailResponse>>(result);

                // For Total Money
                foreach (R2900OrderDetailResponse item in r2900OrderDetailResponses)
                {
                    CostAll += item.Amount * item.PriceOrigin;
                    RevenueAll += item.TotalPrice;
                }

                var totalMoney = RevenueAll - CostAll; 

                return JsonConvert.SerializeObject(totalMoney, Formatting.Indented);
            }

            // Find to amount, IdShop with idOrderShop
            if (what == 2910)
            {
                var id = param.IdOrderShop.Value;
                ParametersCheckAmount paramInput = _mapper.Map<ParametersCheckAmount>(param);
                // Call insert all data to OrderDetail table
                var amountUser = await _d2900OrderDetailDataAccess.SelectWithIdOderShop(paramInput);
                IEnumerable<R2900AmountProduct> r2900AmountProduct = _mapper.Map<IEnumerable<R2900AmountProduct>>(amountUser);
                int numberOrderShop = r2900AmountProduct.Count();
                var isCheckAmout = false;

                if (numberOrderShop == 0)
                {
                    isCheckAmout = true;
                }
                else
                {
                    // Check Amount Product with Warehouse
                    foreach (R2900AmountProduct item in r2900AmountProduct)
                    {
                        if (item.UserAmount > item.WareHouseAmount)
                        {
                            isCheckAmout = true;
                        }
                    }

                }
                var result = new
                {
                    status = isCheckAmout,
                    amountUser = amountUser, 
                }; 

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            //Update Amount --
            if (what == 2911)
            {
                // Get id OrderDetail 
                var id = param.id.Value;

                // Call delete all data OrderDetail table by list id
                var result = await _d2900OrderDetailDataAccess.UpdateAmountMinusAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            if (what == 2912)
            {
                // Get id OrderDetail 
                var id = param.id.Value;

                // Call delete all data OrderDetail table by list id
                var result = await _d2900OrderDetailDataAccess.UpdateAmountPlusAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            //Get data Manager User Product with IdOrderShop
            if (what == 2913)
            {
                // Get id OrderDetail 
                var id = param.id.Value;

                // Call find OrderDetail from table by id
                var result = await _d2900OrderDetailDataAccess.GetByUserManagerProducDetailtAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            //Review Star Order
            if (what == 2914)
            {
                ParametersReviewStar parameters = _mapper.Map<ParametersReviewStar>(param);

                // Call find OrderDetail from table by id
                var result = await _d2900OrderDetailDataAccess.RevierStarOrder(parameters);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }


            return null;
        }
    }
}