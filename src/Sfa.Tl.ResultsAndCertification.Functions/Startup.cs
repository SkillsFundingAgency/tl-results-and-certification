﻿using Lrs.LearnerService.Api.Client;
using Lrs.PersonalLearningRecordService.Api.Client;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Notify.Client;
using Notify.Interfaces;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.PathwayResult;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.Specialism;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Application.Strategies;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Service;
using Sfa.Tl.ResultsAndCertification.Common.Services.Configuration;
using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Functions;
using Sfa.Tl.ResultsAndCertification.Functions.Helpers;
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
        private ResultsAndCertificationConfiguration _configuration;

        public override void Configure(IFunctionsHostBuilder builder)
        {
            _configuration = ConfigurationLoader.Load(
               Environment.GetEnvironmentVariable(Constants.EnvironmentNameConfigKey),
               Environment.GetEnvironmentVariable(Constants.ConfigurationStorageConnectionStringConfigKey),
               Environment.GetEnvironmentVariable(Constants.VersionConfigKey)
               ?? Environment.GetEnvironmentVariable(Constants.ServiceVersionConfigKey), // Need ServiceVersion rather than Version in local with .Net 6
               Environment.GetEnvironmentVariable(Constants.ServiceNameConfigKey));

            RegisterDependencies(builder.Services);
        }

        private void RegisterDependencies(IServiceCollection services)
        {
            //Inject DbContext
            services.AddDbContext<ResultsAndCertificationDbContext>(options =>
                options.UseSqlServer(_configuration.SqlConnectionString,
                    builder => builder.UseNetTopologySuite()
                                      .EnableRetryOnFailure()), ServiceLifetime.Transient);

            services.AddSingleton(_configuration);
            Assembly[] assemblies = new Assembly[] { typeof(Startup).Assembly, typeof(Startup).Assembly.GetReferencedAssemblies().Where(a => a.FullName.Contains("Sfa.Tl.ResultsAndCertification.Application")).Select(Assembly.Load).FirstOrDefault() };
            services.AddAutoMapper(assemblies);
            services.AddHttpContextAccessor();

            RegisterApplicationServices(services);
            RegisterApiClients(services);
        }

        private void RegisterApplicationServices(IServiceCollection services)
        {
            services.AddHttpClient<IDfeSignInApiClient, DfeSignInApiClient>();
            services.AddTransient<ITokenServiceClient, TokenServiceClient>();
            services.AddTransient<IRegistrationRepository, RegistrationRepository>();
            services.AddTransient<IAssessmentRepository, AssessmentRepository>();
            services.AddTransient<IResultRepository, ResultRepository>();
            services.AddTransient<IPrintingRepository, PrintingRepository>();
            services.AddTransient(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<ICommonRepository, CommonRepository>();
            services.AddTransient<IUcasRepository, UcasRepository>();
            services.AddTransient<IProviderRepository, ProviderRepository>();

            services.AddTransient<ICommonService, CommonService>();
            services.AddTransient<ILrsLearnerService, LrsLearnerService>();
            services.AddTransient<ILrsPersonalLearningRecordService, LrsPersonalLearningRecordService>();
            services.AddTransient<ILrsService, LrsService>();
            services.AddTransient<IPrintingService, PrintingService>();
            services.AddTransient<ICertificatePrintingService, CertificatePrintingService>();
            services.AddTransient<IUcasDataService, UcasDataService>();
            services.AddTransient<IUcasDataTransferService, UcasDataTransferService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IAsyncNotificationClient, NotificationClient>(provider => new NotificationClient(_configuration.GovUkNotifyApiKey));
            services.AddTransient<IIndustryPlacementService, IndustryPlacementService>();
            services.AddTransient<IIndustryPlacementNotificationService, IndustryPlacementNotificationService>();
            services.AddTransient<IBlobStorageService, BlobStorageService>();
            services.AddTransient<IProviderAddressExtractionService, ProviderAddressExtractionService>();
            services.AddTransient<ICertificateTrackingExtractionService, CertificateTrackingExtractionService>();

            // Overall result calculation
            services.AddTransient<IOverallResultCalculationFunctionService, OverallResultCalculationFunctionService>();
            services.AddTransient<ISpecialismResultStrategyFactory, SpecialismResultStrategyFactory>();
            services.AddTransient<IOverallGradeStrategyFactory, OverallGradeStrategyFactory>();
            services.AddTransient<IOverallResultCalculationService, OverallResultCalculationService>();
            services.AddTransient<IOverallResultCalculationRepository, OverallResultCalculationRepository>();
            services.AddTransient<IOverallResultRepository, OverallResultRepository>();
            services.AddTransient<IPathwayResultConverter, PathwayResultConverter>();
            services.AddTransient<IIndustryPlacementStatusConverter, IndustryPlacementStatusConverter>();

            // Certificate
            services.AddTransient<ICertificateService, CertificateService>();
            services.AddTransient<ICertificateRepository, CertificateRepository>();

            // Analyst Result
            services.AddTransient<IAnalystOverallResultExtractionService, AnalystOverallResultExtractionService>();
            services.AddTransient<IAnalystCoreResultExtractionService, AnalystCoreResultExtractionService>();

            // ROMM Extract
            services.AddTransient<ICoreRommExtractService, CoreRommExtractService>();
            services.AddTransient<ISpecialismRommExtractionService, SpecialismRommExtractionService>();

            services.AddTransient<SpecialismCodeConverter>();
        }

        private void RegisterApiClients(IServiceCollection services)
        {
            var lrsCertificate = Common.Services.Certificates.CertificateService.GetLearningRecordServiceCertificate(_configuration).GetAwaiter().GetResult();

            services.AddTransient<ILearnerServiceR9Client>(learnerClient =>
            {
                var client = new LearnerServiceR9Client();
                client.Endpoint.Address = CommonHelper.GetLrsEndpointAddress(_configuration.LearningRecordServiceSettings.BaseUri, ApiConstants.PlrServiceUri);
                client.ClientCredentials.ClientCertificate.Certificate = lrsCertificate;
                return client;
            });
            services.AddTransient<ILrsPersonalLearningRecordServiceApiClient, LrsPersonalLearningRecordServiceApiClient>();

            services.AddTransient<ILearnerPortTypeClient>(learnerClient =>
            {
                var client = new LearnerPortTypeClient();
                client.Endpoint.Address = CommonHelper.GetLrsEndpointAddress(_configuration.LearningRecordServiceSettings.BaseUri, ApiConstants.LearnerServiceUri);
                client.ClientCredentials.ClientCertificate.Certificate = lrsCertificate;
                return client;
            });

            services.AddTransient<ILrsLearnerServiceApiClient, LrsLearnerServiceApiClient>();
            services.AddHttpClient<IPrintingApiClient, PrintingApiClient>();
            services.AddHttpClient<IUcasApiClient, UcasApiClient>();
            services.AddTransient<IUcasRecordSegment<UcasRecordEntriesSegment>, UcasRecordEntriesSegment>();
            services.AddTransient<IUcasRecordSegment<UcasRecordResultsSegment>, UcasRecordResultsSegment>();
        }
    }
}
