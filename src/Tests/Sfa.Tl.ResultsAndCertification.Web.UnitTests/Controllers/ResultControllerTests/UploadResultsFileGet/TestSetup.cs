using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.UploadResultsFileGet
{
    public abstract class TestSetup : BaseTest<ResultController>
    {
        protected int? RequestErrorTypeId;
        protected ILogger<ResultController> Logger;
        protected ResultController Controller;
        protected UploadResultsRequestViewModel ViewModel;
        public IActionResult Result { get; private set; }

        public override void Setup()
        {
            Controller = new ResultController();
            ViewModel = new UploadResultsRequestViewModel();
        }

        public override Task When()
        {
            Result = Controller.UploadResultsFile(RequestErrorTypeId);
            return Task.CompletedTask;
        }
    }
}
