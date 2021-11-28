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
    public class C1000ShopCommentController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID1000ShopCommentDataAccess _d1000ShopCommentDataAccess;

        public C1000ShopCommentController(ID1000ShopCommentDataAccess d1000ShopCommentDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d1000ShopCommentDataAccess = d1000ShopCommentDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data ShopComment
            if (what == 1000)
            {
                // Call get all data ShopComment
                IEnumerable<E1000ShopComment> shopComment = await _d1000ShopCommentDataAccess.GetAllAsync();
               
                return JsonConvert.SerializeObject(shopComment, Formatting.Indented);
            }

            // Insert data to table ShopComment
            if (what == 1001)
            {
                // Auto map request param data to Entity
                var shopComment = _mapper.Map<E1000ShopComment>(param);
                shopComment.CreatedAt = TypeConverterExtension.ToDateTime(Convert.ToString(param.CreatedAt));
                // Call insert all data to ShopComment table
                var result = await _d1000ShopCommentDataAccess.CreateAsync(shopComment);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table ShopComment
            if (what == 1002)
            {
                // Auto map request param data to Entity
                var shopComment = _mapper.Map<E1000ShopComment>(param);
                shopComment.id = param.id.Value;
                shopComment.CreatedAt = TypeConverterExtension.ToDateTime(Convert.ToString(param.CreatedAt));

                // Call insert all data to ShopComment table
                var result = await _d1000ShopCommentDataAccess.UpdateAsync(shopComment);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data ShopComment by Id
            if (what == 1003)
            {
                // Get id ShopComment need delete
                var listid = param.listid.Value;

                // Call delete all data ShopComment table by list id
                var result = await _d1000ShopCommentDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data ShopComment by Id
            if (what == 1004)
            {
                // Get id ShopComment need delete
                var id = param.id.Value;

                // Call find ShopComment from table by id
                var result = await _d1000ShopCommentDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data ShopComment Pagination 
            if (what == 1005)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from ShopComment table have pagination
                var result = await _d1000ShopCommentDataAccess.GetPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check ShopComment exists by Id
            if (what == 1006)
            {
                // Get id ShopComment need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }
                // Call check ShopComment in table
                var result = await _d1000ShopCommentDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            if (what == 1007)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from ShopComment table have pagination
                var result = await _d1000ShopCommentDataAccess.GetPaginationShopCommentAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }
            //count number item comment
            if (what == 1008)
            {
                // Get id ShopComment need check
                var Condition = "";
                if (param.condition != null)
                {
                    Condition = param.condition;
                }
                // Call check ShopComment in table
                var result = await _d1000ShopCommentDataAccess.CountNumberItemComment(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            //get data comment parent
            if (what == 1009)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from ShopComment table have pagination
                var result = await _d1000ShopCommentDataAccess.GetPaginationParentAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }
            //get data comment child
            if (what == 1010)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param); 

                // Call get all data from ShopComment table have pagination
                var result = await _d1000ShopCommentDataAccess.GetPaginationChildAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }
            //insert comment product
            if (what == 1011)
            {
                // Auto map request param data to Entity
                ParametersShopComment queryParam = _mapper.Map<ParametersShopComment>(param); 

                // Call get all data from ShopComment table have pagination
                var result = await _d1000ShopCommentDataAccess.GetInsertShopCommentAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            return null;
        }
    }
}