using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.AddProviderTlevels
{
    public abstract class TestSetup : BaseTest<ProviderController>
    {
        protected long Ukprn;
        protected int ProviderId;
        protected IProviderLoader ProviderLoader;
        protected ProviderController Controller;
        protected Task<IActionResult> Result;
        protected ILogger<ProviderController> Logger;
        protected IHttpContextAccessor HttpContextAccessor;

        public override void Setup()
        {
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            ProviderLoader = Substitute.For<IProviderLoader>();
            Logger = Substitute.For<ILogger<ProviderController>>();
            Controller = new ProviderController(ProviderLoader, Logger);
        }

        public async override Task When()
        {
            Result = Controller.AddProviderTlevelsAsync(ProviderId);
        }
    }
}
