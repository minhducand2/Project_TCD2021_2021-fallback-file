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
    public class C3000CommentStatusController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID3000CommentStatusDataAccess _d3000CommentStatusDataAccess;

        public C3000CommentStatusController(ID3000CommentStatusDataAccess d3000CommentStatusDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d3000CommentStatusDataAccess = d3000CommentStatusDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data CommentStatus
            if (what == 3000)
            {
                // Call get all data CommentStatus
                IEnumerable<E3000CommentStatus> commentStatus = await _d3000CommentStatusDataAccess.GetAllAsync();
               
                return JsonConvert.SerializeObject(commentStatus, Formatting.Indented);
            }

            // Insert data to table CommentStatus
            if (what == 3001)
            {
                // Auto map request param data to Entity
                var commentStatus = _mapper.Map<E3000CommentStatus>(param);

                // Call insert all data to CommentStatus table
                var result = await _d3000CommentStatusDataAccess.CreateAsync(commentStatus);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table CommentStatus
            if (what == 3002)
            {
                // Auto map request param data to Entity
                var commentStatus = _mapper.Map<E3000CommentStatus>(param);
                commentStatus.id = param.id.Value;

                // Call insert all data to CommentStatus table
                var result = await _d3000CommentStatusDataAccess.UpdateAsync(commentStatus);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data CommentStatus by Id
            if (what == 3003)
            {
                // Get id CommentStatus need delete
                var listid = param.listid.Value;

                // Call delete all data CommentStatus table by list id
                var result = await _d3000CommentStatusDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data CommentStatus by Id
            if (what == 3004)
            {
                // Get id CommentStatus need delete
                var id = param.id.Value;

                // Call find CommentStatus from table by id
                var result = await _d3000CommentStatusDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data CommentStatus Pagination 
            if (what == 3005)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from CommentStatus table have pagination
                var result = await _d3000CommentStatusDataAccess.GetPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check CommentStatus exists by Id
            if (what == 3006)
            {
                // Get id CommentStatus need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }
                // Call check CommentStatus in table
                var result = await _d3000CommentStatusDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            } 

            return null;
        }
    }
}