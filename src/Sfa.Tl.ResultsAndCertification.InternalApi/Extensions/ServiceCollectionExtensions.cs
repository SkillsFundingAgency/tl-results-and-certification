using System;
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
            var key = Encoding.ASCII.GetBytes(configuration.ResultsAndCertificationApiSettings.InternalApiSecret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration.ResultsAndCertificationApiSettings.InternalApiIssuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }

        public static IServiceCollection AddApiAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.DefaultPolicy = policy;
            });
            return services;
        }
    }
}
