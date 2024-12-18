using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminNotificationControllerTests
{
    public abstract class AdminNotificationControllerBaseTest : BaseTest<AdminProviderController>
    {
        // Dependencies
        protected IAdminNotificationLoader AdminNotificationLoader;
        protected ICacheService CacheService;
        protected ILogger<AdminNotificationController> Logger;
        protected AdminNotificationController Controller;

        // HttpContext
        protected IHttpContextAccessor HttpContextAccessor;
        protected string CacheKey;
        protected string NotificationCacheKey;

        public override void Setup()
        {
            AdminNotificationLoader = Substitute.For<IAdminNotificationLoader>();
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<AdminNotificationController>>();
            Controller = new AdminNotificationController(AdminNotificationLoader, CacheService, Logger);

            var httpContext = new ClaimsIdentityBuilder<AdminNotificationController>(Controller)
               .Add(CustomClaimTypes.Ukprn, "1234567890")
               .Add(CustomClaimTypes.UserId, "b3f2e1d4-5a6b-4c8e-9f3d-7a1b2c3d4e5f")
               .Build()
               .HttpContext;

            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            HttpContextAccessor.HttpContext.Returns(httpContext);

            CacheKey = CacheKeyHelper.GetCacheKey(httpContext.User.GetUserId(), CacheConstants.AdminNotificationCacheKey);
            NotificationCacheKey = CacheKeyHelper.GetCacheKey(httpContext.User.GetUserId(), CacheConstants.AdminNotificationInformationCacheKey);
        }

        public override void Given()
        {
        }

        protected static FilterLookupData CreateFilter(int id, string name, bool isSelected = false)
           => new()
           {
               Id = id,
               Name = name,
               IsSelected = isSelected
           };
    }
}