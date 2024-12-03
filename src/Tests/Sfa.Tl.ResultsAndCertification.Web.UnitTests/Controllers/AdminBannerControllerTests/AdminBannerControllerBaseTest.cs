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

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminBannerControllerTests
{
    public abstract class AdminBannerControllerBaseTest : BaseTest<AdminProviderController>
    {
        // Dependencies
        protected IAdminBannerLoader AdminBannerLoader;
        protected ICacheService CacheService;
        protected ILogger<AdminBannerController> Logger;
        protected AdminBannerController Controller;

        // HttpContext
        protected IHttpContextAccessor HttpContextAccessor;
        protected string CacheKey;
        protected string NotificationCacheKey;

        public override void Setup()
        {
            AdminBannerLoader = Substitute.For<IAdminBannerLoader>();
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<AdminBannerController>>();
            Controller = new AdminBannerController(AdminBannerLoader, CacheService, Logger);

            var httpContext = new ClaimsIdentityBuilder<AdminBannerController>(Controller)
               .Add(CustomClaimTypes.Ukprn, "1234567890")
               .Add(CustomClaimTypes.UserId, "b3f2e1d4-5a6b-4c8e-9f3d-7a1b2c3d4e5f")
               .Build()
               .HttpContext;

            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            HttpContextAccessor.HttpContext.Returns(httpContext);

            CacheKey = CacheKeyHelper.GetCacheKey(httpContext.User.GetUserId(), CacheConstants.AdminBannerCacheKey);
            NotificationCacheKey = CacheKeyHelper.GetCacheKey(httpContext.User.GetUserId(), CacheConstants.AdminBannerInformationCacheKey);
        }

        public override void Given()
        {
        }
    }
}
