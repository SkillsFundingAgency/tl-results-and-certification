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

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests
{
    public abstract class AdminPostResultsControllerTestBase : BaseTest<AdminChangeLogController>
    {
        // Dependencies
        protected IAdminPostResultsLoader AdminPostResultsLoader;
        protected ICacheService CacheService;
        protected ILogger<AdminPostResultsController> Logger;

        protected string CacheKey;
        protected AdminPostResultsController Controller;

        public override void Setup()
        {
            AdminPostResultsLoader = Substitute.For<IAdminPostResultsLoader>();
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<AdminPostResultsController>>();
            Controller = new AdminPostResultsController(AdminPostResultsLoader, CacheService, Logger);

            var httpContext = new ClaimsIdentityBuilder<AdminPostResultsController>(Controller)
               .Add(CustomClaimTypes.Ukprn, "1234567890")
               .Add(CustomClaimTypes.UserId, "82bbe986-d264-460c-ab89-d241c1025f12")
               .Build()
               .HttpContext;

            IHttpContextAccessor httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext.Returns(httpContext);

            CacheKey = CacheKeyHelper.GetCacheKey(httpContext.User.GetUserId(), CacheConstants.AdminPostResultsCacheKey);
        }

        public override void Given()
        {
        }
    }
}