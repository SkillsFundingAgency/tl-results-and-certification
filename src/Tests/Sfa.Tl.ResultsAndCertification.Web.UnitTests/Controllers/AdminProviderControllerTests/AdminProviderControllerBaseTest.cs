using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminProviderControllerTests
{
    public abstract class AdminProviderControllerBaseTest : BaseTest<AdminProviderController>
    {
        // Dependencies
        protected IAdminProviderLoader AdminProviderLoader;
        protected IProviderLoader ProviderLoader;
        protected ICacheService CacheService;
        protected AdminProviderController Controller;

        // HttpContext
        protected IHttpContextAccessor HttpContextAccessor;
        protected string CacheKey;
        protected string NotificationCacheKey;

        public override void Setup()
        {
            AdminProviderLoader = Substitute.For<IAdminProviderLoader>();
            ProviderLoader = Substitute.For<IProviderLoader>();
            CacheService = Substitute.For<ICacheService>();
            Controller = new AdminProviderController(ProviderLoader, AdminProviderLoader, CacheService);

            var httpContext = new ClaimsIdentityBuilder<AdminProviderController>(Controller)
               .Add(CustomClaimTypes.Ukprn, "1234567890")
               .Add(CustomClaimTypes.UserId, "123e4567-e89b-12d3-a456-426614174000")
               .Build()
               .HttpContext;

            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            HttpContextAccessor.HttpContext.Returns(httpContext);

            CacheKey = CacheKeyHelper.GetCacheKey(httpContext.User.GetUserId(), CacheConstants.AdminDashboardCacheKey);
            NotificationCacheKey = CacheKeyHelper.GetCacheKey(httpContext.User.GetUserId(), CacheConstants.AdminDashboardInformationCacheKey);
        }

        public override void Given()
        {
        }
    }
}
