using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using ServiceDiscovery;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FinancePlanner.TaxServices.Services.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddSwaggerVersions();
            services.AddSecurity(config);
            services.AddServiceDiscovery(config);
            services.AddHealthChecks(config);
            return services;
        }

        // Health Check
        private static void AddHealthChecks(this IServiceCollection services, IConfiguration config)
        {
        }

        // Security
        private static void AddSecurity(this IServiceCollection services, IConfiguration config)
        {
            //services.AddAuthentication(config.GetSection("Authentication:Scheme").Value)
            //    .AddJwtBearer(config.GetSection("Authentication:Scheme").Value, options =>
            //    {
            //        options.Authority = config.GetSection("Authentication:IdentityServer:Url").Value;
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateAudience = false
            //        };
            //    });

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy(config.GetSection("Authentication:Policy:Name").Value,
            //        policy => policy.RequireClaim(config.GetSection("Authentication:Policy:ClaimType").Value,
            //            config.GetSection("Authentication:Policy:AllowedValues").Value));
            //});
        }
        
        // Service Discovery
        private static void AddServiceDiscovery(this IServiceCollection services, IConfiguration config)
        {
            ServiceConfig serviceConfig = config.GetServiceConfig();
            services.RegisterConsulServices(serviceConfig);
        }

        // Swagger
        private static void AddSwaggerVersions(this IServiceCollection services)
        {
            // Swagger extensions
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerExtension>();

            services.AddApiVersioning(options =>
            {
                // Specify the default API Version as 1.0
                options.DefaultApiVersion = new ApiVersion(1, 0);
                // If the client hasn't specified the API version in the request, use the default API version number 
                options.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API versions supported for the particular endpoint
                options.ReportApiVersions = true;
                // HTTP Header based versions or query based
                // c.ApiVersionReader = ApiVersionReader.Combine(new HeaderApiVersionReader("x-api-version"),
                // new QueryStringApiVersionReader("api-version"));
            });

            services.AddVersionedApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddSwaggerGenNewtonsoftSupport();

            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"Enter 'Bearer' [space] and your token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                
                options.AddSecurityRequirement(new OpenApiSecurityRequirement 
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });
        }
    }
}
