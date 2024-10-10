using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.PostResultsServiceControllerTests.UploadRommsFileGet
{
    public abstract class TestSetup : PostResultsServiceControllerTestBase
    {
        protected int? RequestErrorTypeId;
        protected UploadRommsRequestViewModel ViewModel;
        public IActionResult Result { get; private set; }

        public override void Setup()
        {
            PostResultsServiceLoader = Substitute.For<IPostResultsServiceLoader>();
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<PostResultsServiceController>>();
            Controller = new PostResultsServiceController(PostResultsServiceLoader, CacheService, Logger);
            ViewModel = new UploadRommsRequestViewModel();
        }

        public override Task When()
        {
            Result = Controller.UploadRommsFile(RequestErrorTypeId);
            return Task.CompletedTask;
        }
    }
}
