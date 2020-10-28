using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.UploadAssessmentsFileGet
{
    public abstract class TestSetup : BaseTest<RegistrationController>
    {
        protected AssessmentController Controller;
        protected UploadAssessmentsRequestViewModel ViewModel;
        public IActionResult Result { get; private set; }

        public override void Setup()
        {
            Controller = new AssessmentController();
            ViewModel = new UploadAssessmentsRequestViewModel();
        }

        public override Task When()
        {
            Result = Controller.UploadAssessmentsFile();
            return Task.CompletedTask;
        }
    }
}
