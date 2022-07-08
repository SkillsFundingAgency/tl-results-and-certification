using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DownloadResultsControllerTests
{
    public abstract class DownloadResultsControllerTestBase : BaseTest<DownloadResultsController>
    {
        // Dependencies
        protected DownloadResultsController Controller;

        // HttpContext
        protected int ProviderUkprn;
        protected Guid UserId;
        protected IHttpContextAccessor HttpContextAccessor;

        public override void Setup()
        {
            Controller = new DownloadResultsController();

            ProviderUkprn = 1234567890;
            var httpContext = new ClaimsIdentityBuilder<DownloadResultsController>(Controller)
               .Add(CustomClaimTypes.Ukprn, ProviderUkprn.ToString())
               .Add(CustomClaimTypes.UserId, Guid.NewGuid().ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            HttpContextAccessor.HttpContext.Returns(httpContext);
        }
    }
}
