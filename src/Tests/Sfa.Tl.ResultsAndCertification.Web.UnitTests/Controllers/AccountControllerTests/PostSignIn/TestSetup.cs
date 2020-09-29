using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using System.Threading.Tasks;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AccountControllerTests.PostSignIn
{
    public abstract class TestSetup : BaseTest<AccountController>
    {
        protected AccountController Controller;
        protected IActionResult Result;
        protected ILogger<AccountController> Logger;
        protected HttpContext HttpContext;
        protected IServiceProvider ServiceProvider;
        protected ICacheService CacheService;
        
        public ResultsAndCertificationConfiguration Configuration { get; set; }

        public override void Setup()
        {
            HttpContext = Substitute.For<HttpContext>();
            ServiceProvider = Substitute.For<IServiceProvider>();
            ServiceProvider.GetService(typeof(IUrlHelperFactory)).Returns(new UrlHelperFactory());
            HttpContext.RequestServices = ServiceProvider;
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<AccountController>>();
            Controller = new AccountController(Configuration, CacheService, Logger);
        }

        public override Task When()
        {
            Result = Controller.PostSignIn();
            return Task.CompletedTask;
        }
    }
}
