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
    public class C000AccountController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID000AccountDataAccess _d000AccountDataAccess;

        public C000AccountController(ID000AccountDataAccess d000AccountDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d000AccountDataAccess = d000AccountDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data Account
            if (what == 0)
            {
                // Auto map request param data to Entity
                var account = _mapper.Map<E000Account>(param);

                // Call get all data Account
                IEnumerable<E000Account> result = await _d000AccountDataAccess.CheckLogin(account);
                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Insert data to table Account
            if (what == 1)
            {
                // Auto map request param data to Entity
                var account = _mapper.Map<E000Account>(param); 
                account.IdRole = param.IdRole.Value;
                account.created_date = TypeConverterExtension.ToDateTime(Convert.ToString(param.created_date));
                // Call insert all data to Account table
                var result = await _d000AccountDataAccess.CreateAsync(account);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table Account
            if (what == 2)
            {
                // Auto map request param data to Entity
                var account = _mapper.Map<E000Account>(param);
                account.id = param.id.Value;
                account.IdRole = param.IdRole.Value;
                account.created_date1 = TypeConverterExtension.ToDateTime(Convert.ToString(param.created_date1));

                // Call insert all data to Account table
                var result = await _d000AccountDataAccess.UpdateAsync(account);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data Account by Id
            if (what == 3)
            {
                // Get id Account need delete
                var id = param.listid.Value;

                // Call delete all data Account table by list id
                var result = await _d000AccountDataAccess.DeleteAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data Account by email
            if (what == 4)
            {
                // Get id Account need delete
                var gmail = param.gmail.Value;

                // Call find Account from table by id
                var result = await _d000AccountDataAccess.GetAccountByMailAsync(gmail);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data Account by Id
            if (what == 6)
            {
                // Get id Account need delete
                string id = param.id;

                // Call find Account from table by id
                var result = await _d000AccountDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Change password
            if (what == 7)
            {
                // Auto map request param data to Entity
                var account = _mapper.Map<E000Account>(param);
                account.id = param.id.Value;

                // Call find Account from table by id
                var result = await _d000AccountDataAccess.ChangePassword(account);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update Info Staff
            if (what == 8)
            {
                // Auto map request param data to Entity
                var account = _mapper.Map<E000Account>(param);
                account.id = param.id.Value;

                // Call find Account from table by id
                var result = await _d000AccountDataAccess.UpdateInfoStaff(account);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update Avatar
            if (what == 9)
            {
                // Auto map request param data to Entity
                var account = _mapper.Map<E000Account>(param);
                account.id = param.id.Value;

                // Call find Account from table by id
                var result = await _d000AccountDataAccess.UpdateAvatar(account);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }


            // Get data Account Pagination 
            if (what == 10)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from Account table have pagination
                var result = await _d000AccountDataAccess.GetAccountsPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            } 

            // Count Accounts
            if (what == 11)
            { 
                // Call check Account in table
                var result = await _d000AccountDataAccess.CountNumberItem();

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            if (what == 12)
            {
                // Call get all data ShopComment
                IEnumerable<E000Account> account = await _d000AccountDataAccess.GetAllAsync();

                return JsonConvert.SerializeObject(account, Formatting.Indented);
            }

            if (what == 13)
            {
                // Auto map request param data to Entity
                var account = _mapper.Map<E000Account>(param);

                // Call get all data Account
                IEnumerable<E000Account> result = await _d000AccountDataAccess.CheckEmail(account);
                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            return null;
        }
    }
}