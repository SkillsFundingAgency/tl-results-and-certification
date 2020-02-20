using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiAuthentication(this IServiceCollection services, ResultsAndCertificationConfiguration configuration)
        {
            // configure jwt authentication
            services.AddAuthentication(x =>
            {
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Authority = $"https://login.microsoftonline.com/{configuration.ResultsAndCertificationInternalApiSettings.TenantId}";
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudiences = new List<string>
                    {
                        configuration.ResultsAndCertificationInternalApiSettings.IdentifierUri,
                        configuration.ResultsAndCertificationInternalApiSettings.ClientId
                    }
                };
            });

            return services;
        }

        public static IServiceCollection AddApiAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApplicationPolicy", policy => policy.RequireRole("Application"));
            });
            return services;
        }
    }
}
