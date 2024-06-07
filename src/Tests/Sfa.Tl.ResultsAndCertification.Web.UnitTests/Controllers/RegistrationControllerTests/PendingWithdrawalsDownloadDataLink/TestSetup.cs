using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.PendingWithdrawalsDownloadDataLink
{
    public abstract class TestSetup : BaseTest<RegistrationController>
    {
        protected string Id;

        protected const long Ukprn = 123456789;
        protected IRegistrationLoader RegistrationLoader;

        protected IActionResult Result { get; private set; }

        private RegistrationController _controller;

        public override void Setup()
        {
            RegistrationLoader = Substitute.For<IRegistrationLoader>();

            _controller = new RegistrationController(
                RegistrationLoader,
                Substitute.For<ICacheService>(),
                Substitute.For<ILogger<RegistrationController>>());

            var httpContext = new ClaimsIdentityBuilder<RegistrationController>(_controller)
               .Add(CustomClaimTypes.Ukprn, Ukprn.ToString())
               .Build()
               .HttpContext;

            IHttpContextAccessor httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext.Returns(httpContext);
        }

        public async override Task When()
        {
            Result = await _controller.PendingWithdrawalsDownloadDataLinkAsync(Id);
        }
    }
}