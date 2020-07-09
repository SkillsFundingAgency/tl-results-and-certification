using AutoMapper;
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
            ResultsAndCertificationConfiguration.EnableLocalAuthentication = false;

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
                config.Filters.Add<CustomExceptionFilterAttribute>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            if(_env.IsDevelopment())
            {
                //services.AddSingleton<IDistributedCache, InMemoryCache>();
                builder.AddRazorRuntimeCompilation();
            }

            services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(_env.IsDevelopment() ? "localhost" : ResultsAndCertificationConfiguration.RedisSettings.CacheConnection));
            services.AddSingleton<IDistributedCache, RedisCache>();

            services.AddWebAuthentication(ResultsAndCertificationConfiguration, _env);
            services.AddAuthorization(options =>
            {
                options.AddPolicy(RolesExtensions.RequireTLevelsReviewerAccess, policy => policy.RequireRole(RolesExtensions.SiteAdministrator, RolesExtensions.TlevelsReviewer));
                options.AddPolicy(RolesExtensions.RequireProviderEditorAccess, policy => policy.RequireRole(RolesExtensions.SiteAdministrator, RolesExtensions.ProvidersEditor));
                options.AddPolicy(RolesExtensions.RequireRegistrationsEditorAccess, policy => policy.RequireRole(RolesExtensions.SiteAdministrator, RolesExtensions.RegistrationsEditor));
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseXContentTypeOptions();
            app.UseReferrerPolicy(opts => opts.NoReferrer());
            app.UseXXssProtection(opts => opts.EnabledWithBlockMode());
            app.UseXfo(xfo => xfo.Deny());
            app.UseCsp(options => options.DefaultSources(s => s.Self()).ScriptSources(s => s.Self().UnsafeInline()));
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
        }
    }
}
