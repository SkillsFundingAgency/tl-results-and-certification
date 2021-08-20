using Microsoft.AspNetCore.Http;
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

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ReportIssueAsyncGet
{
    public abstract class TestSetup : BaseTest<TlevelController>
    {
        protected long AoUkprn;
        protected string CacheKey;
        protected ITlevelLoader TlevelLoader;
        protected ICacheService CacheService;
        protected ILogger<TlevelController> Logger;
        protected TlevelController Controller;
        protected IHttpContextAccessor HttpContextAccessor;
        public TempDataDictionary TempData { get; private set; }
        protected IActionResult Result;
        protected int pathwayId;

        protected TlevelQueryViewModel expectedResult;

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

            TempData = new TempDataDictionary(HttpContextAccessor.HttpContext, Substitute.For<ITempDataProvider>());
            Controller.TempData = TempData;

            expectedResult = new TlevelQueryViewModel
            {
                PathwayId = 1,
                PathwayName = "Test Pathway",
                PathwayStatusId = 1,
                Query = "Test query",
                Specialisms = new List<string> { "Spl1", "Spl2" },
                TqAwardingOrganisationId = pathwayId
            };
        }

        public async override Task When()
        {
            Result = await Controller.ReportIssueAsync(pathwayId);
        }
    }
}
