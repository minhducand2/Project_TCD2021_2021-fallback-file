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
    public class C2800OrderShopController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID2800OrderShopDataAccess _d2800OrderShopDataAccess;
        private ID2900OrderDetailDataAccess _d2900OrderDetailDataAccess;

        public C2800OrderShopController(ID2800OrderShopDataAccess d2800OrderShopDataAccess, ID2900OrderDetailDataAccess d2900OrderDetailDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d2800OrderShopDataAccess = d2800OrderShopDataAccess;
            _d2900OrderDetailDataAccess = d2900OrderDetailDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data OrderShop
            if (what == 2800)
            {
                // Call get all data OrderShop
                IEnumerable<E2800OrderShop> orderShop = await _d2800OrderShopDataAccess.GetAllAsync();
               
                return JsonConvert.SerializeObject(orderShop, Formatting.Indented);
            }

            // Insert data to table OrderShop
            if (what == 2801)
            {
                // Auto map request param data to Entity
                var orderShop = _mapper.Map<E2800OrderShop>(param);
                orderShop.CreatedAt = TypeConverterExtension.ToDateTime(Convert.ToString(param.CreatedAt));
                orderShop.UpdatedAt = TypeConverterExtension.ToDateTime(Convert.ToString(param.UpdatedAt));

                // Call insert all data to OrderShop table
                var result = await _d2800OrderShopDataAccess.CreateAsync(orderShop);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table OrderShop
            if (what == 2802)
            {
                // Auto map request param data to Entity
                var orderShop = _mapper.Map<E2800OrderShop>(param);
                orderShop.id = param.id.Value;
                orderShop.CreatedAt = TypeConverterExtension.ToDateTime(Convert.ToString(param.CreatedAt));
                orderShop.UpdatedAt = TypeConverterExtension.ToDateTime(Convert.ToString(param.UpdatedAt));

                // Call insert all data to OrderShop table
                var result = await _d2800OrderShopDataAccess.UpdateAsync(orderShop);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data OrderShop by Id
            if (what == 2803)
            {
                // Get id OrderShop need delete
                var listid = param.listid.Value;

                // Call delete all data OrderShop table by list id
                var result = await _d2800OrderShopDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data OrderShop by Id
            if (what == 2804)
            {
                // Get id OrderShop need delete
                var id = param.id.Value;

                // Call find OrderShop from table by id
                var result = await _d2800OrderShopDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data OrderShop Pagination 
            if (what == 2805)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from OrderShop table have pagination
                var result = await _d2800OrderShopDataAccess.GetPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check OrderShop exists by Id
            if (what == 2806)
            {
                // Get id OrderShop need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }
                // Call check OrderShop in table
                var result = await _d2800OrderShopDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            //Amout Order Date with status = 1
            if (what == 2807)
            {
                // Auto map request param data to Entity         
                ParametersDateTime parameters = _mapper.Map<ParametersDateTime>(param);
                parameters.startDate = param.startDate;
                parameters.startDate1 = param.startDate + " 00:00:00";
                parameters.endDate = param.endDate;
                parameters.endDate1 = param.endDate + " 23:59:59";


                // Call insert all data to User table
                var result = await _d2800OrderShopDataAccess.CustomJoinOrder1Date(parameters);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            //Amout Order Date with status = 2
            if (what == 2808)
            {
                // Auto map request param data to Entity         
                ParametersDateTime parameters = _mapper.Map<ParametersDateTime>(param);
                parameters.startDate = param.startDate;
                parameters.startDate1 = param.startDate + " 00:00:00";
                parameters.endDate = param.endDate;
                parameters.endDate1 = param.endDate + " 23:59:59";


                // Call insert all data to User table
                var result = await _d2800OrderShopDataAccess.CustomJoinOrder2Date(parameters);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            //Amout Order Date with status = 3
            if (what == 2809)
            {
                // Auto map request param data to Entity         
                ParametersDateTime parameters = _mapper.Map<ParametersDateTime>(param);
                parameters.startDate = param.startDate;
                parameters.startDate1 = param.startDate + " 00:00:00";
                parameters.endDate = param.endDate;
                parameters.endDate1 = param.endDate + " 23:59:59";


                // Call insert all data to User table
                var result = await _d2800OrderShopDataAccess.CustomJoinOrder3Date(parameters);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            //Amout Order Date with status = 4
            if (what == 2810)
            {
                // Auto map request param data to Entity         
                ParametersDateTime parameters = _mapper.Map<ParametersDateTime>(param);
                parameters.startDate = param.startDate;
                parameters.startDate1 = param.startDate + " 00:00:00";
                parameters.endDate = param.endDate;
                parameters.endDate1 = param.endDate + " 23:59:59";


                // Call insert all data to User table
                var result = await _d2800OrderShopDataAccess.CustomJoinOrder4Date(parameters);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }
            //Amout Order Date with status = 5
            if (what == 2811)
            {
                // Auto map request param data to Entity         
                ParametersDateTime parameters = _mapper.Map<ParametersDateTime>(param);
                parameters.startDate = param.startDate;
                parameters.startDate1 = param.startDate + " 00:00:00";
                parameters.endDate = param.endDate;
                parameters.endDate1 = param.endDate + " 23:59:59";


                // Call insert all data to User table
                var result = await _d2800OrderShopDataAccess.CustomJoinOrder5Date(parameters);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            //Amout Order Date with status All
            if (what == 2812)
            {
                // Auto map request param data to Entity         
                ParametersDateTime parameters = _mapper.Map<ParametersDateTime>(param);
                parameters.startDate = param.startDate;
                parameters.startDate1 = param.startDate + " 00:00:00";
                parameters.endDate = param.endDate;
                parameters.endDate1 = param.endDate + " 23:59:59";


                // Call insert all data to User table
                var result = await _d2800OrderShopDataAccess.CustomJoinOrderAllDate(parameters);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find Count Cart OrderShop by IdUser
            if (what == 2813)
            {
                // Get id OrderShop need delete
                var IdUser = param.IdUser.Value;

                // Call find OrderShop from table by id
                var result = await _d2800OrderShopDataAccess.GetByIdUserCountCartAsync(IdUser);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            //Find OrderShop With IdUser Cart
            if (what == 2814)
            {
                // Auto map request param data to Entity         
                ParametersOrderShop parameters = _mapper.Map<ParametersOrderShop>(param); 

                // Call insert all data to User table
                var result = await _d2800OrderShopDataAccess.GetOrderShopWithUser(parameters);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            if (what == 2815)
            {
                // Get id OrderShop need delete Cart
                var id = param.IdUser.Value;

                // Call find OrderShop from table by id
                var result = await _d2800OrderShopDataAccess.GetByUserCartAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update TotalPrice
            if (what == 2816)
            {
                // Auto map request param data to Entity
                ParametersOderShopCart parameters = _mapper.Map<ParametersOderShopCart>(param);

                // Call insert all data to OrderShop table
                var result = await _d2800OrderShopDataAccess.UpdateTotalPriceAsync(parameters);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table OrderShop
            if (what == 2817)
            {
                // Auto map request param data to Entity
                var orderShop = _mapper.Map<ParametersOderShopCart>(param);
                orderShop.id = param.id.Value; 
                orderShop.UpdatedAt = TypeConverterExtension.ToDateTime(Convert.ToString(param.UpdatedAt));

                // Call insert all data to OrderShop table
                var result = await _d2800OrderShopDataAccess.UpdatePayAsync(orderShop);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get Data Product Manager User
            if (what == 2818)
            {
               
                var id = param.IdUser.Value;

                // Call find OrderShop from table by id
                var result = await _d2800OrderShopDataAccess.GetByUserManagerProductAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get Data Product Manager User Received
            if (what == 2819)
            {

                var id = param.IdUser.Value;

                // Call find OrderShop from table by id
                var result = await _d2800OrderShopDataAccess.GetByUserOrderReceivedAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get Data Product Manager User refuse
            if (what == 2820)
            {

                var id = param.IdUser.Value;

                // Call find OrderShop from table by id
                var result = await _d2800OrderShopDataAccess.GetByUserOrderRefuseAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            //Order Shop Reset
            if (what == 2821)
            {
                // Auto map request param data to Entity
                ParametersOrderShopReset parameters = _mapper.Map<ParametersOrderShopReset>(param); 

                // Call find OrderShop from table by id
                var result = await _d2800OrderShopDataAccess.GetByOrederShopResetAsync(parameters); 

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Cancel Order
            if (what == 2822)
            {

                var id = param.id;

                // Call find OrderShop from table by id
                var result = await _d2800OrderShopDataAccess.CancelOrderAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }



            return null;
        }
    }
}