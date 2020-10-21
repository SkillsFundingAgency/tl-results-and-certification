using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AccountControllerTests.Profile
{
    public abstract class TestSetup : BaseTest<AccountController>
    {
        protected AccountController Controller;
        protected IActionResult Result;
        protected ILogger<AccountController> Logger;
        protected IHttpContextAccessor HttpContextAccessor;
        protected ICacheService CacheService;

        public ResultsAndCertificationConfiguration Configuration { get; set; }

        public override void Setup()
        {
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            Logger = Substitute.For<ILogger<AccountController>>();
            CacheService = Substitute.For<ICacheService>();
        }

        public override Task When()
        {
            Controller = new AccountController(Configuration, CacheService, Logger);
            Result = Controller.Profile();
            return Task.CompletedTask;
        }
    }
}