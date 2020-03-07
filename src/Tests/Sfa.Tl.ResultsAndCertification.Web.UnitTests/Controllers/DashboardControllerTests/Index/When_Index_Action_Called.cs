using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DashboardControllerTests.Index
{
    public abstract class When_Index_Action_Called : BaseTest<DashboardController>
    {
        protected DashboardController Controller;
        protected IActionResult Result;
        protected IHttpContextAccessor HttpContextAccessor;

        public override void Setup()
        {
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            Controller = new DashboardController();
        }

        public override void When()
        {
            Result = Controller.Index();
        }
    }
}
