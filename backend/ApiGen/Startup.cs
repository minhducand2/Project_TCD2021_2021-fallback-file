using ApiGen.Infrastructure.Configs;
using ApiGen.Infrastructure.Extensions;
using AspNetCoreRateLimit;
using AutoMapper;
using AutoWrapper;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.IO; 

namespace ApiGen
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Register services in Installers folder
            services.AddServicesInAssembly(Configuration);

            //Register MVC/Web API, NewtonsoftJson and add FluentValidation Support
            services.AddControllers()
                    .AddNewtonsoftJson(ops => { ops.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore; })
                    .AddFluentValidation(fv => { fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false; });

            services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            }); 

            //Register Automapper
            services.AddAutoMapper(typeof(MappingProfileConfiguration));

            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@"/home/izicloud4/web-ban-hang"))
                .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
                {
                    EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                    ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
                }); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // app.UseHttpsRedirection();

            //Enable Swagger and SwaggerUI
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiGen ASP.NET Core API v1");
            });

            //Enable HealthChecks and UI
            // app.UseHealthChecks("/selfcheck", new HealthCheckOptions
            // {
            //    Predicate = _ => true,
            //    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            // }).UseHealthChecksUI(setup =>
            // {
            //    setup.AddCustomStylesheet($"{env.ContentRootPath}/Infrastructure/HealthChecks/Ux/branding.css");
            // });

            //Enable AutoWrapper.Core
            //More info see: https://github.com/proudmonkey/AutoWrapper
            //app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions { IsDebug = true, UseApiProblemDetailsException = true});

            //Enable AspNetCoreRateLimit
            app.UseIpRateLimiting();

            app.UseRouting();

            app.UseStaticFiles();
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Resources")),
            //    RequestPath = new PathString("/Resources")
            //});

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
             Path.Combine(env.WebRootPath, "images")),
                RequestPath = "/wwwroot"
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.WebRootPath, "images")),
                RequestPath = "/wwwroot"
            });

            // Enable CORS
            app.UseCors("AllowAll"); 

            //Adds authenticaton middleware to the pipeline so authentication will be performed automatically on each request to host
            app.UseAuthentication();

            //Adds authorization middleware to the pipeline to make sure the Api endpoint cannot be accessed by anonymous clients
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
