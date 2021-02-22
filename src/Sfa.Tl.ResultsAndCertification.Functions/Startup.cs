using AutoMapper;
using Lrs.LearnerService.Api.Client;
using Lrs.PersonalLearningRecordService.Api.Client;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Certificates;
using Sfa.Tl.ResultsAndCertification.Common.Services.Configuration;
using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Functions;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Functions.Services;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System;
using System.Linq;
using System.Reflection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Sfa.Tl.ResultsAndCertification.Functions
{
    public class Startup : FunctionsStartup
    {
        protected ResultsAndCertificationConfiguration ResultsAndCertificationConfiguration;
        
        public override void Configure(IFunctionsHostBuilder builder)
        {
            ResultsAndCertificationConfiguration = ConfigurationLoader.Load(
               Environment.GetEnvironmentVariable(Constants.EnvironmentNameConfigKey),
               Environment.GetEnvironmentVariable(Constants.ConfigurationStorageConnectionStringConfigKey),
               Environment.GetEnvironmentVariable(Constants.VersionConfigKey),
               Environment.GetEnvironmentVariable(Constants.ServiceNameConfigKey));

            RegisterDependencies(builder.Services);
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
            services.AddHttpContextAccessor();

            RegisterApplicationServices(services);
            RegisterApiClients(services);
        }

        private void RegisterApplicationServices(IServiceCollection services)
        {
            services.AddTransient<IRegistrationRepository, RegistrationRepository>();
            services.AddTransient<IAssessmentRepository, AssessmentRepository>();
            services.AddTransient<IResultRepository, ResultRepository>();
            services.AddTransient(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();

            services.AddTransient<ICommonService, CommonService>();
            services.AddTransient<ILearnerRecordService, LearnerRecordService>();
        }

        private void RegisterApiClients(IServiceCollection services)
        {
            var lrsCertificate = CertificateService.GetLearningRecordServiceCertificate(ResultsAndCertificationConfiguration).GetAwaiter().GetResult();

            services.AddTransient<ILearnerServiceR9Client>(learnerClient =>
            {
                var client = new LearnerServiceR9Client();
                client.ClientCredentials.ClientCertificate.Certificate = lrsCertificate;
                return client;
            });
            services.AddTransient<IPersonalLearningRecordApiClient, PersonalLearningRecordServiceClient>();
        }
    }
}
