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
    public class C2000UserController
    {
        private readonly ILogger<dynamic> _logger;
        private readonly IMapper _mapper;
        private ID2000UserDataAccess _d2000UserDataAccess;

        public C2000UserController(ID2000UserDataAccess d2000UserDataAccess, IMapper mapper, ILogger<dynamic> logger)
        {
            _d2000UserDataAccess = d2000UserDataAccess;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> execute(int what, dynamic param)
        {
            // Get all data User
            if (what == 2000)
            {
                // Call get all data User
                IEnumerable<E2000User> user = await _d2000UserDataAccess.GetAllAsync();
               
                return JsonConvert.SerializeObject(user, Formatting.Indented);
            }

            // Insert data to table User
            if (what == 2001)
            {
                // Auto map request param data to Entity
                var user = _mapper.Map<E2000User>(param);
                user.CreatedAt = TypeConverterExtension.ToDateTime(Convert.ToString(param.CreatedAt));
                user.UpdatedAt = TypeConverterExtension.ToDateTime(Convert.ToString(param.UpdatedAt));
                // Call insert all data to User table
                var result = await _d2000UserDataAccess.CreateAsync(user);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Update data table User
            if (what == 2002)
            {
                // Auto map request param data to Entity
                var user = _mapper.Map<E2000User>(param);
                user.id = param.id.Value;
                user.CreatedAt = TypeConverterExtension.ToDateTime(Convert.ToString(param.CreatedAt));
                user.UpdatedAt = TypeConverterExtension.ToDateTime(Convert.ToString(param.UpdatedAt));

                // Call insert all data to User table
                var result = await _d2000UserDataAccess.UpdateAsync(user);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Delete data User by Id
            if (what == 2003)
            {
                // Get id User need delete
                var listid = param.listid.Value;

                // Call delete all data User table by list id
                var result = await _d2000UserDataAccess.DeleteAsync(listid);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data User by Id
            if (what == 2004)
            {
                // Get id User need delete
                var id = param.id.Value;

                // Call find User from table by id
                var result = await _d2000UserDataAccess.GetByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Get data User Pagination 
            if (what == 2005)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param);
                queryParam.limit = unchecked((int)param.limit.Value);
                queryParam.offset = unchecked((int)param.offset.Value);

                // Call get all data from User table have pagination
                var result = await _d2000UserDataAccess.GetPaginationAsync(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Check User exists by Id
            if (what == 2006)
            {
                // Get id User need check
                var Condition = "";
                if (param.Condition != null)
                {
                    Condition = param.Condition;
                }
                // Call check User in table
                var result = await _d2000UserDataAccess.CountNumberItem(Condition);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Count user Date
            if (what == 2007)
            {
                // Auto map request param data to Entity     
                ParametersDateTime parameters = _mapper.Map<ParametersDateTime>(param);
                    parameters.startDate = param.startDate;
                    parameters.startDate1 = param.startDate + " 00:00:00";
                    parameters.endDate = param.endDate;
                    parameters.endDate1 = param.endDate + " 23:59:59";

                // Call insert all data to User table
                var result = await _d2000UserDataAccess.CustomJoinUserDate(parameters);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data User With Login
            if (what == 2008)
            {
                // Auto map request param data to Entity
                UrlQueryParameters queryParam = _mapper.Map<UrlQueryParameters>(param); 

                // Call get all data from User table have pagination
                var result = await _d2000UserDataAccess.GetUserLogin(queryParam);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }
            // Insert data to Registration User
            if (what == 2009)
            {
                // Auto map request param data to Entity
                var user = _mapper.Map<E2000User>(param); 
                // Call insert all data to User table
                var idUser = await _d2000UserDataAccess.CreateAsyncLogin(user); 

                return JsonConvert.SerializeObject(idUser, Formatting.Indented);
            }

            // Insert data to Registration User
            if (what == 2010)
            {
                // Auto map request param data to Entity
                var user = _mapper.Map<E2000User>(param); 
                // Call insert all data to User table
                var idUser = await _d2000UserDataAccess.CreateAsyncRegistration(user);

                return JsonConvert.SerializeObject(idUser, Formatting.Indented);
            }


            // Find data User by Email
            if (what == 2011)
            {
                // Get id User need delete
                var Email = param.Email.Value;

                // Call find User from table by id
                var result = await _d2000UserDataAccess.GetByWithEmailAsync(Email);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            // Find data User by Email & Password
            if (what == 2012)
            {
                var parameters = _mapper.Map<E2000User>(param);

                // Call find User from table by id
                var result = await _d2000UserDataAccess.GetByLoginAsync(parameters);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }
            // Update Password data table User
            if (what == 2013)
            {
                // Auto map request param data to Entity
                var user = _mapper.Map<E2000User>(param);
                user.id = param.id.Value; 
                // Call insert all data to User table
                var result = await _d2000UserDataAccess.UpdatePasswordAsync(user);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            if (what == 2014)
            {
                // Get id User need delete
                var id = param.id.Value;

                // Call find User from table by id
                var result = await _d2000UserDataAccess.GetPointByIdAsync(id);

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }


            return null;
        }
    }
}