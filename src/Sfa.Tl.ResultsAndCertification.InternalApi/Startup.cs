using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notify.Client;
using Notify.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.PathwayResult;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Application.Services.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Strategies;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Service;
using Sfa.Tl.ResultsAndCertification.Common.Services.Configuration;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataValidators;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service.Interface;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Service;
using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Data.Builder;
using Sfa.Tl.ResultsAndCertification.Data.Factory;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.InternalApi.Extensions;
using Sfa.Tl.ResultsAndCertification.InternalApi.Infrastructure;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.PostResultsService.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess;
using System.Configuration;
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
            ResultsAndCertificationConfiguration = ConfigurationLoader.Load(Configuration);

            services.AddApplicationInsightsTelemetry();
            services.AddControllers();
            services.AddSwaggerGen();

            services.Configure<ApiBehaviorOptions>(options =>
            {
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

            services
                .AddHealthChecks()
                .AddDbContextCheck<ResultsAndCertificationDbContext>();
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

                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseAuthentication();
                app.UseAuthorization();
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
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

            ValidatorOptions.Global.DisplayNameResolver = (type, memberInfo, expression) =>
            {
                return memberInfo.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.GetName();
            };
        }

        private void RegisterApplicationServices(IServiceCollection services)
        {
            // Repositories
            services.AddTransient<IBlobStorageService, BlobStorageService>();
            services.AddTransient<IBlobClientFactory, BlobClientFactory>();
            services.AddTransient<IProviderRepository, ProviderRepository>();
            services.AddTransient<IRegistrationRepository, RegistrationRepository>();
            services.AddTransient<IAssessmentRepository, AssessmentRepository>();
            services.AddTransient<IResultRepository, ResultRepository>();
            services.AddTransient<ITrainingProviderRepository, TrainingProviderRepository>();
            services.AddTransient<IStatementOfAchievementRepository, StatementOfAchievementRepository>();
            services.AddTransient<IPostResultsServiceRepository, PostResultsServiceRepository>();
            services.AddTransient<ILearnerRepository, LearnerRepository>();
            services.AddTransient<ICommonRepository, CommonRepository>();
            services.AddTransient<IAdminDashboardRepository, AdminDashboardRepository>();
            services.AddTransient<IAdminChangeLogRepository, AdminChangeLogRepository>();
            services.AddTransient<ISearchRegistrationRepository, SearchRegistrationRepository>();
            services.AddTransient<IProviderRegistrationsRepository, ProviderRegistrationsRepository>();
            services.AddTransient(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IRepositoryFactory, RepositoryFactory>();

            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IAwardingOrganisationService, AwardingOrganisationService>();
            services.AddTransient<IProviderService, ProviderService>();
            services.AddTransient<IPathwayService, PathwayService>();
            services.AddTransient<IDbContextBuilder, DbContextBuilder>();
            services.AddTransient<IAsyncNotificationClient, NotificationClient>(provider => new NotificationClient(ResultsAndCertificationConfiguration.GovUkNotifyApiKey));
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IDocumentUploadHistoryService, DocumentUploadHistoryService>();
            services.AddTransient<ICommonService, CommonService>();
            services.AddTransient<ISystemProvider, SystemProvider>();

            // Bulk Registrations
            services.AddTransient<IDataParser<RegistrationCsvRecordResponse>, RegistrationParser>();
            services.AddTransient<IValidator<RegistrationCsvRecordRequest>, RegistrationValidator>();
            services.AddTransient<IDataParser<WithdrawalCsvRecordResponse>, WithdrawalParser>();
            services.AddTransient<IValidator<WithdrawalCsvRecordRequest>, WithdrawalValidator>();
            services.AddTransient<ICsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse>, CsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse>>();
            services.AddTransient<ICsvHelperService<WithdrawalCsvRecordRequest, CsvResponseModel<WithdrawalCsvRecordResponse>, WithdrawalCsvRecordResponse>, CsvHelperService<WithdrawalCsvRecordRequest, CsvResponseModel<WithdrawalCsvRecordResponse>, WithdrawalCsvRecordResponse>>();
            services.AddTransient<IBulkBaseLoader, BulkBaseLoader>();
            services.AddTransient<IBulkProcessLoader, BulkRegistrationLoader>();
            services.AddTransient<IBulkWithdrawalLoader, BulkWithdrawalLoader>();
            services.AddTransient<IRegistrationService, RegistrationService>();
            services.AddTransient<IWithdrawalService, WithdrawalService>();


            services.AddTransient<IDataParser<RommCsvRecordResponse>, RommParser>();
            services.AddTransient<IValidator<RommsCsvRecordRequest>, RommValidator>();
            services.AddTransient<ICsvHelperService<RommsCsvRecordRequest, CsvResponseModel<RommCsvRecordResponse>, RommCsvRecordResponse>, CsvHelperService<RommsCsvRecordRequest, CsvResponseModel<RommCsvRecordResponse>, RommCsvRecordResponse>>();
            services.AddTransient<IBulkRommLoader, BulkRommLoader>();
            services.AddTransient<IRommService, RommService>();

            // Bulk Assessments
            services.AddTransient<IDataParser<AssessmentCsvRecordResponse>, AssessmentParser>();
            services.AddTransient<IValidator<AssessmentCsvRecordRequest>, AssessmentValidator>();
            services.AddTransient<ICsvHelperService<AssessmentCsvRecordRequest, CsvResponseModel<AssessmentCsvRecordResponse>, AssessmentCsvRecordResponse>, CsvHelperService<AssessmentCsvRecordRequest, CsvResponseModel<AssessmentCsvRecordResponse>, AssessmentCsvRecordResponse>>();
            services.AddTransient<IBulkAssessmentLoader, BulkAssessmentLoader>();
            services.AddTransient<IAssessmentService, AssessmentService>();

            // Bulk Results
            services.AddTransient<IDataParser<ResultCsvRecordResponse>, ResultParser>();
            services.AddTransient<IValidator<ResultCsvRecordRequest>, ResultValidator>();
            services.AddTransient<ICsvHelperService<ResultCsvRecordRequest, CsvResponseModel<ResultCsvRecordResponse>, ResultCsvRecordResponse>, CsvHelperService<ResultCsvRecordRequest, CsvResponseModel<ResultCsvRecordResponse>, ResultCsvRecordResponse>>();
            services.AddTransient<IBulkResultLoader, BulkResultLoader>();
            services.AddTransient<IResultService, ResultService>();

            // Application Services
            services.AddTransient<ITrainingProviderService, TrainingProviderService>();
            services.AddTransient<IProviderAddressService, ProviderAddressService>();
            services.AddTransient<IStatementOfAchievementService, StatementOfAchievementService>();
            services.AddTransient<IPostResultsServiceService, PostResultsServiceService>();
            services.AddTransient<ILearnerService, LearnerService>();
            services.AddTransient<IAdminDashboardService, AdminDashboardService>();
            services.AddTransient<IAdminChangeLogService, AdminChangeLogService>();
            services.AddTransient<IAdminPostResultsService, AdminPostResultsService>();
            services.AddTransient<ISearchRegistrationService, SearchRegistrationService>();
            services.AddTransient<IProviderRegistrationsService, ProviderRegistrationsService>();

            // DataExports 
            services.AddTransient<IDataExportLoader, DataExportLoader>();
            services.AddTransient<IDataExportRepository, DataExportRepository>();

            // IndustryPlacement
            services.AddTransient<IDataParser<IndustryPlacementCsvRecordResponse>, IndustryPlacementParser>();
            services.AddTransient<IValidator<IndustryPlacementCsvRecordRequest>, IndustryPlacementValidator>();
            services.AddTransient<ICsvHelperService<IndustryPlacementCsvRecordRequest, CsvResponseModel<IndustryPlacementCsvRecordResponse>, IndustryPlacementCsvRecordResponse>, CsvHelperService<IndustryPlacementCsvRecordRequest, CsvResponseModel<IndustryPlacementCsvRecordResponse>, IndustryPlacementCsvRecordResponse>>();
            services.AddTransient<IBulkIndustryPlacementLoader, BulkIndustryPlacementLoader>();
            services.AddTransient<IIndustryPlacementService, IndustryPlacementService>();

            // Overall result calculation
            services.AddTransient<ISpecialismResultStrategyFactory, SpecialismResultStrategyFactory>();
            services.AddTransient<IOverallGradeStrategyFactory, OverallGradeStrategyFactory>();
            services.AddTransient<IOverallResultCalculationService, OverallResultCalculationService>();
            services.AddTransient<IOverallResultRepository, OverallResultRepository>();
            services.AddTransient<IOverallResultCalculationRepository, OverallResultCalculationRepository>();

            // Converter
            services.AddTransient<IPathwayResultConverter, PathwayResultConverter>();
            services.AddTransient<IIndustryPlacementStatusConverter, IndustryPlacementStatusConverter>();

            // Certificate Printing Service
            services.AddTransient<ICertificateService, CertificateService>();
            services.AddTransient<ICertificateRepository, CertificateRepository>();
        }
    }
}