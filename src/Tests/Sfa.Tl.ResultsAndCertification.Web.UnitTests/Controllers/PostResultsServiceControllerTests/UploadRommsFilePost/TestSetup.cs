using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.PostResultsServiceControllerTests.UploadWithdrawlsFilePost
{
    public abstract class TestSetup : PostResultsServiceControllerTestBase
    {
        protected long Ukprn;
        protected UploadRommsRequestViewModel ViewModel;
        protected UploadRommsResponseViewModel ResponseViewModel;
        protected IFormFile FormFile;
        public IActionResult Result { get; private set; }
        protected Guid BlobUniqueReference;

        public override void Setup()
        {
            Ukprn = 12345;
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            PostResultsServiceLoader = Substitute.For<IPostResultsServiceLoader>();
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<PostResultsServiceController>>();
            Controller = new PostResultsServiceController(PostResultsServiceLoader, CacheService, Logger);
            ViewModel = new UploadRommsRequestViewModel();

            var httpContext = new ClaimsIdentityBuilder<PostResultsServiceController>(Controller)
               .Add(CustomClaimTypes.Ukprn, Ukprn.ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
            TempData = new TempDataDictionary(HttpContextAccessor.HttpContext, Substitute.For<ITempDataProvider>());
            Controller.TempData = TempData;
        }

        public async override Task When()
        {
            Result = await Controller.UploadRommsFileAsync(ViewModel);
        }
    }
}
