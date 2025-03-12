using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.HelpControllerTests.ServiceUnavailableAwardingOrganisation
{
    public abstract class TestSetup : BaseTest<HelpController>
    {
        public ResultsAndCertificationConfiguration Configuration { get; set; }
        public IHelpLoader HelpLoader;
        protected IActionResult Result;
        protected HelpController Controller;
        protected IHttpContextAccessor HttpContextAccessor;

        public override void Setup()
        {
            HelpLoader = new HelpLoader();
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();

            ServiceFreezePeriods serviceFreezePeriods = new()
            {
                AwardingOrganisation = new()
                {
                    EndDate = new DateTime(2022, 07, 31, 23, 59, 59, DateTimeKind.Utc)
                }
            };

            Configuration = new ResultsAndCertificationConfiguration
            {
                ServiceFreezePeriodsSettings = serviceFreezePeriods
            };

            Controller = new HelpController(Configuration, HelpLoader);

            var httpContext = new ClaimsIdentityBuilder<HelpController>(Controller)
               .Add(CustomClaimTypes.LoginUserType, "1")
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
        }

        public override Task When()
        {
            Result = Controller.ServiceUnavailable();
            return Task.CompletedTask;
        }
    }
}
