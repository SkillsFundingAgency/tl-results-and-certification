using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.Index
{
    public abstract class TestSetup : BaseTest<ResultController>
    {
        protected IResultLoader ResultLoader;

        protected ResultController Controller;
        public IActionResult Result { get; private set; }

        public override void Setup()
        {
            ResultLoader = Substitute.For<IResultLoader>();
            Controller = new ResultController(ResultLoader);
        }

        public override Task When()
        {
            Result = Controller.Index();
            return Task.CompletedTask;
        }
    }
}
