using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Service;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Common.Services.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Web.Authentication;
using Sfa.Tl.ResultsAndCertification.Web.Filters;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.Session;
using Sfa.Tl.ResultsAndCertification.Web.WebConfigurationHelper;
using StackExchange.Redis;
using System;
using System.Globalization;

namespace Sfa.Tl.ResultsAndCertification.Web
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        protected ResultsAndCertificationConfiguration ResultsAndCertificationConfiguration;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _config = configuration;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ResultsAndCertificationConfiguration = ConfigurationLoader.Load(
               _config[Constants.EnvironmentNameConfigKey],
               _config[Constants.ConfigurationStorageConnectionStringConfigKey],
               _config[Constants.VersionConfigKey],
               _config[Constants.ServiceNameConfigKey]);

            ResultsAndCertificationConfiguration.IsDevevelopment = _env.IsDevelopment();
            ResultsAndCertificationConfiguration.BypassDfeSignIn = false;

            services.AddApplicationInsightsTelemetry();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            
            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = "tl-rc-x-csrf";
                options.FormFieldName = "_csrfToken";
                options.HeaderName = "X-XSRF-TOKEN";
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            services.AddSingleton(ResultsAndCertificationConfiguration);
            services.AddTransient<ITokenServiceClient, TokenServiceClient>();
            services.AddTransient<ISessionService, SessionService>();
            services.AddHttpClient<IResultsAndCertificationInternalApiClient, ResultsAndCertificationInternalApiClient>();
            services.AddHttpClient<IOrdnanceSurveyApiClient, OrdnanceSurveyApiClient>();            
            services.AddHttpClient<IDfeSignInApiClient, DfeSignInApiClient>();

            var builder = services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                config.Filters.Add(new AuthorizeFilter(policy));
                config.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
                config.Filters.Add(new ResponseCacheAttribute
                {
                    NoStore = true,
                    Location = ResponseCacheLocation.None
                });
                config.Filters.Add<SessionActivityFilterAttribute>();
                config.Filters.Add<CustomExceptionFilterAttribute>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            if (_env.IsDevelopment())
            {
                //services.AddSingleton<IDistributedCache, InMemoryCache>();
                builder.AddRazorRuntimeCompilation();
            }

            services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(_env.IsDevelopment() ? "localhost" : ResultsAndCertificationConfiguration.RedisSettings.CacheConnection));
            services.AddSingleton<ICacheService, RedisCacheService>();

            services.AddWebAuthentication(ResultsAndCertificationConfiguration, _env);
            services.AddAuthorization(options =>
            {
                // Awarding Organisation Access Policies
                options.AddPolicy(RolesExtensions.RequireTLevelsReviewerAccess, policy => { policy.RequireRole(RolesExtensions.SiteAdministrator, RolesExtensions.TlevelsReviewer); policy.RequireClaim(CustomClaimTypes.LoginUserType, ((int)LoginUserType.AwardingOrganisation).ToString()); });
                options.AddPolicy(RolesExtensions.RequireProviderEditorAccess, policy => { policy.RequireRole(RolesExtensions.SiteAdministrator, RolesExtensions.ProvidersEditor); policy.RequireClaim(CustomClaimTypes.LoginUserType, ((int)LoginUserType.AwardingOrganisation).ToString()); });
                options.AddPolicy(RolesExtensions.RequireRegistrationsEditorAccess, policy => { policy.RequireRole(RolesExtensions.SiteAdministrator, RolesExtensions.RegistrationsEditor); policy.RequireClaim(CustomClaimTypes.LoginUserType, ((int)LoginUserType.AwardingOrganisation).ToString()); });
                options.AddPolicy(RolesExtensions.RequireResultsEditorAccess, policy => { policy.RequireRole(RolesExtensions.SiteAdministrator, RolesExtensions.ResultsEditor); policy.RequireClaim(CustomClaimTypes.LoginUserType, ((int)LoginUserType.AwardingOrganisation).ToString()); });

                // Training Provider Access Policies                
                options.AddPolicy(RolesExtensions.RequireLearnerRecordsEditorAccess, policy => { policy.RequireRole(RolesExtensions.ProviderAdministrator, RolesExtensions.LearnerRecordsEditor); policy.RequireClaim(CustomClaimTypes.LoginUserType, ((int)LoginUserType.TrainingProvider).ToString()); });
            });

            services.AddWebDataProtection(ResultsAndCertificationConfiguration, _env);
            RegisterDependencies(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            var cultureInfo = new CultureInfo("en-GB");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error/500");
                app.UseHsts(options => options.MaxAge(365));
            }

            app.UseXContentTypeOptions();
            app.UseReferrerPolicy(opts => opts.NoReferrer());
            app.UseXXssProtection(opts => opts.EnabledWithBlockMode());
            app.UseXfo(xfo => xfo.Deny());

            app.UseCsp(options => options.ScriptSources(s => s.StrictDynamic()
                                         .CustomSources("https:","https://www.google-analytics.com/analytics.js",
                                                        "https://www.googletagmanager.com/",
                                                        "https://tagmanager.google.com/")
                                         .UnsafeInline())
                                         .ObjectSources(s => s.None()));
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStatusCodePagesWithRedirects("/Error/{0}");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
            app.UseCookiePolicy();
        }

        private void RegisterDependencies(IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSingleton<IWebConfigurationService, WebConfigurationService>();
            services.AddTransient<IBlobStorageService, BlobStorageService>();
            services.AddTransient<ITlevelLoader, TlevelLoader>();
            services.AddTransient<IProviderLoader, ProviderLoader>();
            services.AddTransient<IRegistrationLoader, RegistrationLoader>();
            services.AddTransient<IAssessmentLoader, AssessmentLoader>();
            services.AddTransient<IResultLoader, ResultLoader>();
            services.AddTransient<IDocumentLoader, DocumentLoader>();
            services.AddTransient<ITrainingProviderLoader, TrainingProviderLoader>();
            services.AddTransient<IProviderAddressLoader, ProviderAddressLoader>();
            services.AddTransient<IStatementOfAchievementLoader, StatementOfAchievementLoader>();
        }
    }
}