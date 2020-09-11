using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DashboardControllerTests.Index
{
    public abstract class TestSetup : BaseTest<DashboardController>
    {
        protected ILogger<DashboardController> Logger;
        protected DashboardController Controller;
        protected IActionResult Result;
        protected IHttpContextAccessor HttpContextAccessor;

        public override void Setup()
        {
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            Logger = Substitute.For<ILogger<DashboardController>>();
            Controller = new DashboardController(Logger);
        }

        public override void When()
        {
            Result = Controller.Index();
        }
    }
}
