using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Azure.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notify.Client;
using Notify.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Application.Services.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Service;
using Sfa.Tl.ResultsAndCertification.Common.Services.Configuration;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataValidators;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service.Interface;
using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Data.Builder;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.InternalApi.Extensions;
using Sfa.Tl.ResultsAndCertification.InternalApi.Infrastructure;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Sfa.Tl.ResultsAndCertification.InternalApi
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        protected ResultsAndCertificationConfiguration ResultsAndCertificationConfiguration;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
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
            services.AddAutoMapper(typeof(Startup).Assembly.GetReferencedAssemblies().Where(a => a.FullName.Contains("Sfa.Tl.ResultsAndCertification.Application")).Select(Assembly.Load));
            RegisterApplicationServices(services);

            ValidatorOptions.Global.DisplayNameResolver = (type, memberInfo, expression) => {
                return memberInfo.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.GetName();
            };
        }

        private void RegisterApplicationServices(IServiceCollection services)
        {
            services.AddTransient<IBlobStorageService, BlobStorageService>();
            services.AddTransient<IProviderRepository, ProviderRepository>();
            services.AddTransient<IRegistrationRepository, RegistrationRepository>();
            services.AddTransient(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IAwardingOrganisationService, AwardingOrganisationService>();
            services.AddTransient<IProviderService, ProviderService>();
            services.AddTransient<IPathwayService, PathwayService>();
            services.AddTransient<IDbContextBuilder, DbContextBuilder>();
            services.AddTransient<IAsyncNotificationClient, NotificationClient>(provider => new NotificationClient(ResultsAndCertificationConfiguration.GovUkNotifyApiKey));
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IDocumentUploadHistoryService, DocumentUploadHistoryService>();

            services.AddTransient<IDataParser<RegistrationCsvRecordResponse>, RegistrationParser>();
            services.AddTransient<IValidator<RegistrationCsvRecordRequest>, RegistrationValidator>();
            services.AddTransient<ICsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse>, CsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse>>();
            services.AddTransient<IBulkRegistrationLoader, BulkRegistrationLoader>();
            services.AddTransient<IRegistrationService, RegistrationService>();
        }
    }
}