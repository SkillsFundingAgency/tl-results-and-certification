using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ErrorControllerTests
{
    public abstract class TestSetup : BaseTest<ErrorController>
    {
        protected ErrorController Controller;
        protected IActionResult Result;
        protected ILogger<ErrorController> Logger;
        protected HttpContext HttpContext;
        protected IServiceProvider ServiceProvider;

        private readonly ITempDataProvider _tempDataProvider = Substitute.For<ITempDataProvider>();

        public ResultsAndCertificationConfiguration Configuration { get; set; }

        public override void Setup()
        {
            HttpContext = Substitute.For<HttpContext>();
            ServiceProvider = Substitute.For<IServiceProvider>();
            ServiceProvider.GetService(typeof(IUrlHelperFactory)).Returns(new UrlHelperFactory());
            HttpContext.RequestServices = ServiceProvider;
            Logger = Substitute.For<ILogger<ErrorController>>();
            Controller = new ErrorController(Configuration, Logger);

            var tempDataDictionary = new TempDataDictionary(HttpContext, _tempDataProvider);
            Controller.TempData = tempDataDictionary;
        }

        public override Task When()
        {
            Result = Controller.ServiceAccessDenied();
            return Task.CompletedTask;
        }
    }
}