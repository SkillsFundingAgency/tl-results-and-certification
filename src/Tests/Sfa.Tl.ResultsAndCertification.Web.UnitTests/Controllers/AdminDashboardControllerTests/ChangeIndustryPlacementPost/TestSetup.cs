using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeIndustryPlacementPost
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {
        protected int AoUkprn;
        protected int ProfileId;
        protected Guid UserId;
        protected string CacheKey;
        protected IAdminDashboardLoader AdminDashboardLoader;
        protected ICacheService CacheService;
        protected ILogger<AdminDashboardController> Logger;
        protected AdminDashboardController Controller;
        protected IProviderLoader ProviderLoader;
        protected IHttpContextAccessor HttpContextAccessor;
        protected AdminChangeIndustryPlacementViewModel AdminChangeIndustryPlacementViewModel;

        public IActionResult Result { get; private set; }

        public override void Setup()
        {
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            AdminDashboardLoader = Substitute.For<IAdminDashboardLoader>();
            CacheService = Substitute.For<ICacheService>();
            ProviderLoader = Substitute.For<IProviderLoader>();
            Logger = Substitute.For<ILogger<AdminDashboardController>>();
            Controller = new AdminDashboardController(AdminDashboardLoader, ProviderLoader ,CacheService, Logger);

            ProfileId = 1;
            AoUkprn = 1234567890;
            var httpContext = new ClaimsIdentityBuilder<AdminDashboardController>(Controller)
               .Add(CustomClaimTypes.Ukprn, AoUkprn.ToString())
               .Add(CustomClaimTypes.UserId, Guid.NewGuid().ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
            CacheKey = CacheKeyHelper.GetCacheKey(httpContext.User.GetUserId(), CacheConstants.AdminDashboardCacheKey);
        }

        public async override Task When()
        {
            Result = await Controller.ChangeIndustryPlacementAsync(AdminChangeIndustryPlacementViewModel);
        }
    }
}
