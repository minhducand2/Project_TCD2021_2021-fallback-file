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
    public class C100MenuController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID100MenuDataAccess _d100MenuDataAccess;

        public C100MenuController(ID100MenuDataAccess d100MenuDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d100MenuDataAccess = d100MenuDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data Menu
            if (what == 100)
            {
                // Call get all data Menu
                IEnumerable<E100Menu> sinhVien = await _d100MenuDataAccess.GetAllAsync();
                return JsonConvert.SerializeObject(sinhVien, Formatting.Indented);
            }

            // Insert data to table Menu
            if (what == 101)
            {
                // Auto map request param data to Entity
                var sinhVien = _mapper.Map<E100Menu>(param);

                // Call insert all data to Menu table
                var result = await _d100MenuDataAccess.CreateAsync(sinhVien);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table Menu
            if (what == 102)
            {
                // Auto map request param data to Entity
                var sinhVien = _mapper.Map<E100Menu>(param);
                sinhVien.id = param.id.Value;

                // Call insert all data to Menu table
                var result = await _d100MenuDataAccess.UpdateAsync(sinhVien);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data Menu by id
            if (what == 103)
            {
                // Get id Menu need delete
                var listid = param.listid.Value;

                // Call delete all data Menu table by list id
                var result = await _d100MenuDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data Menu by id
            if (what == 104)
            {
                // Get id Menu need delete
                var id = param.id.Value;

                // Call find Menu from table by id
                var result = await _d100MenuDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data Menu Pagination 
            if (what == 105)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);


                // Call get all data from Menu table have pagination
                var result = await _d100MenuDataAccess.GetMenusPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }


            // Check Menu exists by id
            if (what == 106)
            {
                // Get id Menu need check
                var Condition = "";
                if (param.Condition!=null)
                {
                    Condition = param.Condition;
                }

                // Call check Menu in table
                var result = await _d100MenuDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get all data Menu
            if (what == 107)
            {
                // Call get all data Menu
                IEnumerable<E100Menu> sinhVien = await _d100MenuDataAccess.GetDataByGroup();
                return JsonConvert.SerializeObject(sinhVien, Formatting.Indented);
            }

            // Get all data Menu
            if (what == 108)
            {
                // Call get all data Menu
                IEnumerable<object> result = await _d100MenuDataAccess.GetDataMenuRecusive108();
                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get all data Menu
            if (what == 109)
            {
                // Get id Menu need check
                var idRole = param.idRole;

                // Call get all data Menu
                IEnumerable<object> result = await _d100MenuDataAccess.GetDataMenuRecusive109(idRole);
                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get all data Menu
            if (what == 110)
            {
                // Get id Menu need check
                string idUser = param.idUser;

                // Call get all data Menu
                IEnumerable<E100Menu> result = await _d100MenuDataAccess.GetDataMenuRecusive110(idUser);
                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get all data Menu
            if (what == 111)
            {
                // Get id Menu need check
                var idUser = param.idUser.Value;
                var url = param.url.Value;

                // Call get all data Menu
                IEnumerable<object> result = await _d100MenuDataAccess.GetDataMenuRecusive111(idUser, url);
                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            return null;
        }
    }
}