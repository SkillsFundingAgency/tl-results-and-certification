using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.SearchResultsNotFound
{
    public abstract class TestSetup : BaseTest<ResultController>
    {
        protected int Ukprn;
        protected string Uln;
        protected Guid UserId;
        protected string CacheKey;
        protected IResultLoader ResultLoader;
        protected ICacheService CacheService;
        protected ILogger<ResultController> Logger;
        protected ResultController Controller;
        protected IHttpContextAccessor HttpContextAccessor;

        public IActionResult Result { get; private set; }

        public override void Setup()
        {
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            ResultLoader = Substitute.For<IResultLoader>();
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<ResultController>>();
            Controller = new ResultController(ResultLoader, CacheService, Logger);

            Ukprn = 1234567890;
            Uln = "7765456786";
            var httpContext = new ClaimsIdentityBuilder<ResultController>(Controller)
               .Add(CustomClaimTypes.Ukprn, Ukprn.ToString())
               .Add(CustomClaimTypes.UserId, Guid.NewGuid().ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
        }

        public async override Task When()
        {
            Result = await Controller.SearchResultsNotFoundAsync();
        }
    }
}
