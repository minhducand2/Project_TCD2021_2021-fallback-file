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
    public class C1400BlogController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID1400BlogDataAccess _d1400BlogDataAccess;

        public C1400BlogController(ID1400BlogDataAccess d1400BlogDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d1400BlogDataAccess = d1400BlogDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data Blog
            if (what == 1400)
            {
                // Call get all data Blog
                IEnumerable<E1400Blog> blog = await _d1400BlogDataAccess.GetAllAsync();
               
                return JsonConvert.SerializeObject(blog, Formatting.Indented);
            }

            // Insert data to table Blog
            if (what == 1401)
            {
                // Auto map request param data to Entity
                var blog = _mapper.Map<E1400Blog>(param);
                blog.CreatedAt = TypeConverterExtension.ToDateTime(Convert.ToString(param.CreatedAt));
                blog.UpdatedAt = TypeConverterExtension.ToDateTime(Convert.ToString(param.UpdatedAt));

                // Call insert all data to Blog table
                var result = await _d1400BlogDataAccess.CreateAsync(blog);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table Blog
            if (what == 1402)
            {
                // Auto map request param data to Entity
                var blog = _mapper.Map<E1400Blog>(param);
                blog.id = param.id.Value;
                blog.CreatedAt = TypeConverterExtension.ToDateTime(Convert.ToString(param.CreatedAt));
                blog.UpdatedAt = TypeConverterExtension.ToDateTime(Convert.ToString(param.UpdatedAt));

                // Call insert all data to Blog table
                var result = await _d1400BlogDataAccess.UpdateAsync(blog);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data Blog by Id
            if (what == 1403)
            {
                // Get id Blog need delete
                var listid = param.listid.Value;

                // Call delete all data Blog table by list id
                var result = await _d1400BlogDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data Blog by Id
            if (what == 1404)
            {
                // Get id Blog need delete
                var id = param.id.Value;

                // Call find Blog from table by id
                var result = await _d1400BlogDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data Blog Pagination 
            if (what == 1405)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from Blog table have pagination
                var result = await _d1400BlogDataAccess.GetPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check Blog exists by Id
            if (what == 1406)
            {
                // Get id Blog need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }
                // Call check Blog in table
                var result = await _d1400BlogDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Top Blog LIMIT 0,10   
            if (what == 1407)
            { 
                // Call TOP Blog all data to User table
                var result = await _d1400BlogDataAccess.CustomJoinBlog();

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }


            return null;
        }
    }
}