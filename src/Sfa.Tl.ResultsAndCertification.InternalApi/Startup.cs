using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notify.Client;
using Notify.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Configuration;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Application.Services.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Data.Builder;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.InternalApi.Extensions;
using Sfa.Tl.ResultsAndCertification.InternalApi.Infrastructure;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System;

namespace Sfa.Tl.ResultsAndCertification.InternalApi
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        protected ResultsAndCertificationConfiguration ResultsAndCertificationConfiguration;
        private readonly AzureServiceTokenProvider _tokenProvider;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
            _tokenProvider = new AzureServiceTokenProvider();
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ResultsAndCertificationConfiguration = ConfigurationLoader.Load(
               Configuration[Constants.EnvironmentNameConfigKey],
               Configuration[Constants.ConfigurationStorageConnectionStringConfigKey],
               Configuration[Constants.VersionConfigKey],
               Configuration[Constants.ServiceNameConfigKey]);

            services.AddApplicationInsightsTelemetry();
            services.AddControllers();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                //options.SuppressModelStateInvalidFilter = true;
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    return new BadRequestObjectResult(new BadRequestResponse(actionContext.ModelState));
                };
            });

            RegisterDependencies(services);
            
            if (!_env.IsDevelopment())
            {
                services.AddMvc(config =>
                {
                    config.Filters.Add(new AuthorizeFilter());
                });
                services.AddApiAuthentication(ResultsAndCertificationConfiguration).AddApiAuthorization();
            }

            services.AddApiDataProtection(ResultsAndCertificationConfiguration, _tokenProvider, _env);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.ConfigureExceptionHandlerMiddleware();
            app.UseHttpsRedirection();
            app.UseRouting();

            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseAuthentication();
                app.UseAuthorization();
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void RegisterDependencies(IServiceCollection services)
        {
            //Inject DbContext
            services.AddDbContext<ResultsAndCertificationDbContext>(options =>
                options.UseSqlServer(ResultsAndCertificationConfiguration.SqlConnectionString,
                    builder => builder.UseNetTopologySuite()
                                      .EnableRetryOnFailure()), ServiceLifetime.Transient);

            services.AddSingleton(ResultsAndCertificationConfiguration);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            RegisterApplicationServices(services);
        }

        private void RegisterApplicationServices(IServiceCollection services)
        {
            services.AddTransient<IProviderRepository, ProviderRepository>();
            services.AddTransient(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IAwardingOrganisationService, AwardingOrganisationService>();
            services.AddTransient<IProviderService, ProviderService>();
            services.AddTransient<IPathwayService, PathwayService>();
            services.AddTransient<IDbContextBuilder, DbContextBuilder>();
            services.AddTransient<IAsyncNotificationClient, NotificationClient>(provider => new NotificationClient(ResultsAndCertificationConfiguration.GovUkNotifyApiKey));
            services.AddTransient<INotificationService, NotificationService>();
        }
    }
}