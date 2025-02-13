using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.RommsNoRecordsFound
{
    public abstract class TestSetup : BaseTest<PostResultsServiceController>
    {
        protected int Ukprn;
        protected string Uln;
        protected Guid UserId;
        protected string CacheKey;
        protected IPostResultsServiceLoader PostResultsServiceLoader;
        protected ICacheService CacheService;
        protected ILogger<PostResultsServiceController> Logger;
        protected PostResultsServiceController Controller;
        protected RommsDownloadViewModel ViewModel;
        protected IHttpContextAccessor HttpContextAccessor;
        protected TempDataDictionary TempData;
        public IActionResult Result { get; private set; }

        public override void Setup()
        {
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            PostResultsServiceLoader = Substitute.For<IPostResultsServiceLoader>();
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<PostResultsServiceController>>();
            Controller = new PostResultsServiceController(PostResultsServiceLoader, CacheService, Logger);

            Ukprn = 1234567890;
            Uln = "8765456786";
            var httpContext = new ClaimsIdentityBuilder<PostResultsServiceController>(Controller)
               .Add(CustomClaimTypes.Ukprn, Ukprn.ToString())
               .Add(CustomClaimTypes.UserId, Guid.NewGuid().ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
            TempData = new TempDataDictionary(HttpContextAccessor.HttpContext, Substitute.For<ITempDataProvider>());
            Controller.TempData = TempData;
        }

        public override Task When()
        {
            Result = Controller.RommsNoRecordsFound();
            return Task.CompletedTask;
        }
    }
}
