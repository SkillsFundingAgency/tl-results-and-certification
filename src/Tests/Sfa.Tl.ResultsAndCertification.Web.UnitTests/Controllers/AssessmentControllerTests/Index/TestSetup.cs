using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.Index
{
    public abstract class TestSetup : BaseTest<AssessmentController>
    {
        protected AssessmentController Controller;
        public IActionResult Result { get; private set; }

        public override void Setup()
        {
            Controller = new AssessmentController();
        }

        public override Task When()
        {
            Result = Controller.Index();
            return Task.CompletedTask;
        }
    }
}
