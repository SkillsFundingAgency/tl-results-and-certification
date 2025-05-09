﻿using Microsoft.AspNetCore.Http;
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

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests
{
    public abstract class AdminDashboardControllerTestBase : BaseTest<AdminDashboardController>
    {
        // Dependencies
        protected IAdminDashboardLoader AdminDashboardLoader;
        protected IProviderLoader ProviderLoader;
        protected IIndustryPlacementLoader IndustryPlacementLoader;
        protected ICacheService CacheService;
        protected ILogger<AdminDashboardController> Logger;
        protected AdminDashboardController Controller;

        // HttpContext
        protected int ProviderUkprn;
        protected Guid UserId;
        protected IHttpContextAccessor HttpContextAccessor;
        protected string CacheKey;
        protected string InformationCacheKey;

        public override void Setup()
        {
            AdminDashboardLoader = Substitute.For<IAdminDashboardLoader>();
            ProviderLoader = Substitute.For<IProviderLoader>();
            IndustryPlacementLoader = Substitute.For<IIndustryPlacementLoader>();
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<AdminDashboardController>>();
            Controller = new AdminDashboardController(AdminDashboardLoader, ProviderLoader, IndustryPlacementLoader, CacheService, Logger);

            ProviderUkprn = 1234567890;
            var httpContext = new ClaimsIdentityBuilder<AdminDashboardController>(Controller)
               .Add(CustomClaimTypes.Ukprn, ProviderUkprn.ToString())
               .Add(CustomClaimTypes.UserId, Guid.NewGuid().ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            HttpContextAccessor.HttpContext.Returns(httpContext);

            CacheKey = CacheKeyHelper.GetCacheKey(httpContext.User.GetUserId(), CacheConstants.AdminDashboardCacheKey);
            InformationCacheKey = CacheKeyHelper.GetCacheKey(httpContext.User.GetUserId(), CacheConstants.AdminDashboardCacheKey);
        }

        public override void Given()
        {
        }
    }
}
