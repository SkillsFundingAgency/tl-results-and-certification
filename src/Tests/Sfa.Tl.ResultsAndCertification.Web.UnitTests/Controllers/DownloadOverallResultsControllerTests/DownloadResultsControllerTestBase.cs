using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System;
using System.Security.Claims;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DownloadOverallResultsControllerTests
{
    public abstract class DownloadResultsControllerTestBase : BaseTest<DownloadOverallResultsController>
    {
        // Dependencies
        protected DownloadOverallResultsController Controller;
        protected ResultsAndCertificationConfiguration ResultsAndCertificationConfiguration;
        protected IDownloadOverallResultsLoader DownloadOverallResultsLoader;
        protected ILogger<DownloadOverallResultsController> Logger;


        // HttpContext
        protected int ProviderUkprn = 12345678;
        protected Guid UserId;
        protected string Email = "test.user@test.com";
        protected IHttpContextAccessor HttpContextAccessor;

        public override void Setup()
        {
            ResultsAndCertificationConfiguration = new ResultsAndCertificationConfiguration { OverallResultsAvailableDate = "01/12/2022".ToDateTime() };
            DownloadOverallResultsLoader = Substitute.For<IDownloadOverallResultsLoader>();
            Logger = Substitute.For<ILogger<DownloadOverallResultsController>>();
            Controller = new DownloadOverallResultsController(ResultsAndCertificationConfiguration, DownloadOverallResultsLoader, Logger);

            ProviderUkprn = 1234567890;
            var httpContext = new ClaimsIdentityBuilder<DownloadOverallResultsController>(Controller)
               .Add(CustomClaimTypes.Ukprn, ProviderUkprn.ToString())
               .Add(CustomClaimTypes.UserId, Guid.NewGuid().ToString())
               .Add(ClaimTypes.Email, Email)
               .Build()
               .HttpContext;

            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            HttpContextAccessor.HttpContext.Returns(httpContext);
        }
    }
}
