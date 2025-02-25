using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System;
using System.Security.Claims;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDownloadLearnerResultsControllerTests
{
    public abstract class AdminDownloadLearnerResultsControllerBaseTest : BaseTest<AdminDownloadLearnerResultsController>
    {
        // Dependencies
        protected IProviderLoader ProviderLoader;
        protected IAdminDownloadLearnerResultsLoader AdminDownloadLearnerResultsLoader;
        protected IDownloadOverallResultsLoader DownloadOverallResultsLoader;
        protected ICacheService CacheService;
        protected ILogger<AdminDownloadLearnerResultsController> Logger;

        protected AdminDownloadLearnerResultsController Controller;

        // HttpContext
        protected IHttpContextAccessor HttpContextAccessor;
        protected string CacheKey;

        public override void Setup()
        {
            ProviderLoader = Substitute.For<IProviderLoader>();
            AdminDownloadLearnerResultsLoader = Substitute.For<IAdminDownloadLearnerResultsLoader>();
            DownloadOverallResultsLoader = Substitute.For<IDownloadOverallResultsLoader>();
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<AdminDownloadLearnerResultsController>>();

            Controller = new AdminDownloadLearnerResultsController(ProviderLoader, AdminDownloadLearnerResultsLoader, DownloadOverallResultsLoader, CacheService, Logger);

            var httpContext = new ClaimsIdentityBuilder<AdminDownloadLearnerResultsController>(Controller)
               .Add(CustomClaimTypes.UserId, "ff374550-dc2a-4f7a-8022-f13b944b3e14")
               .Add(ClaimTypes.Email, "test@email.com")
               .Build()
               .HttpContext;

            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            HttpContextAccessor.HttpContext.Returns(httpContext);

            CacheKey = CacheKeyHelper.GetCacheKey(httpContext.User.GetUserId(), CacheConstants.AdminDashboardCacheKey);
        }

        public override void Given()
        {
        }
    }
}