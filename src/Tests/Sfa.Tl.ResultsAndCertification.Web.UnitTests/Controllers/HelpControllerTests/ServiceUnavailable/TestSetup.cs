using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.HelpControllerTests.ServiceUnavailable
{
    public abstract class TestSetup : BaseTest<HelpController>
    {
        public ResultsAndCertificationConfiguration Configuration { get; set; }
        protected IActionResult Result;
        protected HelpController Controller;

        public override void Setup()
        {
            Configuration = new ResultsAndCertificationConfiguration
            {
                FreezePeriodEndDate = "31/07/2022".ToDateTime()
            };

            Controller = new HelpController(Configuration);
        }

        public override Task When()
        {
            Result = Controller.ServiceUnavailable();
            return Task.CompletedTask;
        }
    }
}
