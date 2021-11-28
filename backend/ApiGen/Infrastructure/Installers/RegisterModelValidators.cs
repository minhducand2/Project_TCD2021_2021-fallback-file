using ApiGen.Contracts;
using ApiGen.DTO.Request;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiGen.Infrastructure.Installers
{
    internal class RegisterModelValidators: IServiceRegistration
    {
        public void RegisterAppServices(IServiceCollection services, IConfiguration configuration)
        {
            //Register DTO Validators
            services.AddTransient<IValidator<UpdatePersonRequest>, UpdatePersonRequestValidator>();

            //Disable Automatic Model State Validation built-in to ASP.NET Core
            services.Configure<ApiBehaviorOptions>(opt => { opt.SuppressModelStateInvalidFilter = true; });
        }
    }
}
