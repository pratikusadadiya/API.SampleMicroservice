using API.SampleMicroservice.DataModels.Request;
using API.SampleMicroservice.Endpoints;
using API.SampleMicroservice.Entities;
using API.SampleMicroservice.Interfaces.Repositories;
using API.SampleMicroservice.Interfaces.Repositories.Shared;
using API.SampleMicroservice.Interfaces.Services;
using API.SampleMicroservice.Middlewares;
using API.SampleMicroservice.Repositories;
using API.SampleMicroservice.Repositories.Shared;
using API.SampleMicroservice.Validators;
using API.SampleMicroservice.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Security.Claims;

namespace API.SampleMicroservice.Configuration
{
    public static class Configuration
    {
        public static void ConnectDatabase(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<SampleMicroserviceContext>(options => options.UseNpgsql(config.GetConnectionString("DefaultConnection")));
        }

        public static void AddHttpContext(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(s => s.GetService<IHttpContextAccessor>()?.HttpContext?.User ?? new ClaimsPrincipal());
        }

        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<ISampleEntityService, SampleEntityService>();
        }

        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISampleEntityRepository, SampleEntityRepository>();
        }

        public static void RegisterValidations(this IServiceCollection services)
        {
            services.AddTransient<IValidator<SampleEntityCreateUpdateCommand>, SampleEntityCreateUpdateValidation>();
        }

        public static void RegisterMiddleWares(this IServiceCollection services)
        {
            services.AddTransient<ExceptionMiddleWare>();
        }

        public static void AddSwagger(this IServiceCollection services, string serviceName)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("V1", new OpenApiInfo
                {
                    Version = "V1",
                    Title = serviceName + " WebAPI",
                    Description = "API endpoints for " + serviceName.ToLower()
                });
            });
        }

        public static void RegisterAPIs(this WebApplication app)
        {
            app.RegisterSampleEntityAPIs();

        }
    }
}
