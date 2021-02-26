using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.UploadResultsFileGet
{
    public abstract class TestSetup : BaseTest<ResultController>
    {
        protected IResultLoader ResultLoader;
        protected ICacheService CacheService;
        protected ILogger<ResultController> Logger;

        protected int? RequestErrorTypeId;
        protected ResultController Controller;
        protected UploadResultsRequestViewModel ViewModel;
        public IActionResult Result { get; private set; }

        public override void Setup()
        {
            ResultLoader = Substitute.For<IResultLoader>();
            Controller = new ResultController(ResultLoader, CacheService, Logger);
            ViewModel = new UploadResultsRequestViewModel();
        }

        public override Task When()
        {
            Result = Controller.UploadResultsFile(RequestErrorTypeId);
            return Task.CompletedTask;
        }
    }
}
