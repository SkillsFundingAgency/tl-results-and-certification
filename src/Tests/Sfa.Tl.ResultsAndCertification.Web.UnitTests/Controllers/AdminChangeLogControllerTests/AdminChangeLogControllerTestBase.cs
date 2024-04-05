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

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminChangeLogControllerTests
{
    public abstract class AdminChangeLogControllerTestBase : BaseTest<AdminChangeLogController>
    {
        // Dependencies
        protected IAdminChangeLogLoader AdminChangeLogLoader;
        protected ICacheService CacheService;
        protected ILogger<AdminChangeLogController> Logger;

        protected string CacheKey;
        protected AdminChangeLogController Controller;

        internal readonly int ChangeLogId = 1;

        public override void Setup()
        {
            AdminChangeLogLoader = Substitute.For<IAdminChangeLogLoader>();
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<AdminChangeLogController>>();
            Controller = new AdminChangeLogController(AdminChangeLogLoader, CacheService, Logger);

            var httpContext = new ClaimsIdentityBuilder<AdminChangeLogController>(Controller)
               .Add(CustomClaimTypes.Ukprn, "1234567890")
               .Add(CustomClaimTypes.UserId, "82bbe986-d264-460c-ab89-d241c1025f12")
               .Build()
               .HttpContext;

            IHttpContextAccessor httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext.Returns(httpContext);

            CacheKey = CacheKeyHelper.GetCacheKey(httpContext.User.GetUserId(), CacheConstants.AdminChangeLogCacheKey);
        }

        public override void Given()
        {
        }
    }
}