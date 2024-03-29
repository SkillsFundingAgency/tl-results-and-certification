﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ReportIssueAsyncPost
{
    public abstract class TestSetup : BaseTest<TlevelController>
    {
        protected ITlevelLoader TlevelLoader;
        protected ILogger<TlevelController> Logger;
        protected TlevelController Controller;
        protected IActionResult Result;
        protected ICacheService CacheService;
        protected IHttpContextAccessor HttpContextAccessor;
        protected long AoUkprn;
        protected string CacheKey;
        protected int PathwayId;

        protected TlevelQueryViewModel InputViewModel;
        protected TlevelQueryViewModel ExpectedResult;

        public override void Setup()
        {
            AoUkprn = 1234567890;

            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            TlevelLoader = Substitute.For<ITlevelLoader>();
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<TlevelController>>();
            Controller = new TlevelController(TlevelLoader, CacheService, Logger);

            var httpContext = new ClaimsIdentityBuilder<TlevelController>(Controller)
               .Add(CustomClaimTypes.Ukprn, AoUkprn.ToString())
               .Add(CustomClaimTypes.UserId, Guid.NewGuid().ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
            CacheKey = CacheKeyHelper.GetCacheKey(httpContext.User.GetUserId(), CacheConstants.TlevelCacheKey);

            ExpectedResult = new TlevelQueryViewModel
            {
                TqAwardingOrganisationId = PathwayId,
                RouteId = 2,
                PathwayId = 1,
                PathwayStatusId = 1,

                TlevelTitle = "T Level in Education",
                PathwayDisplayName = "Education (12345678)",
                Specialisms = new List<string> { "Spl1 (11111111)", "Spl2 (22222222)" },

                IsBackToConfirmed = false,
                Query = "Test query",
            };
        }

        public async override Task When()
        {
            Result = await Controller.ReportIssueAsync(InputViewModel);
        }
    }
}
