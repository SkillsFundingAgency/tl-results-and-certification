using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System.Security.Claims;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderRegistrationsControllerTests
{
    public abstract class ProviderRegistrationsControllerTestBase : BaseTest<ProviderRegistrationsController>
    {
        protected IProviderRegistrationsLoader ProviderRegistrationsLoader;
        protected ILogger<ProviderRegistrationsController> Logger;
        protected ProviderRegistrationsController Controller;

        protected const long ProviderUkprn = 1234567890;
        protected const string Email = "test@email.com";
        protected IHttpContextAccessor HttpContextAccessor;

        public override void Setup()
        {
            ProviderRegistrationsLoader = Substitute.For<IProviderRegistrationsLoader>();
            Logger = Substitute.For<ILogger<ProviderRegistrationsController>>();
            Controller = new ProviderRegistrationsController(ProviderRegistrationsLoader, Logger);

            var httpContext = new ClaimsIdentityBuilder<ProviderRegistrationsController>(Controller)
               .Add(CustomClaimTypes.Ukprn, ProviderUkprn.ToString())
               .Add(ClaimTypes.Email, Email)
               .Build()
               .HttpContext;

            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            HttpContextAccessor.HttpContext.Returns(httpContext);
        }
    }
}
