using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.RemoveProviderTlevelGet
{
    public abstract class TestSetup : BaseTest<ProviderController>
    {
        protected long Ukprn;
        protected int TqProviderId;
        protected IProviderLoader ProviderLoader;
        protected ProviderController Controller;
        protected Task<IActionResult> Result;
        protected ILogger<ProviderController> Logger;
        protected IHttpContextAccessor HttpContextAccessor;
        protected TempDataDictionary TempData;

        public override void Setup()
        {
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            ProviderLoader = Substitute.For<IProviderLoader>();
            Logger = Substitute.For<ILogger<ProviderController>>();
            Controller = new ProviderController(ProviderLoader, Logger);
            TempData = new TempDataDictionary(HttpContextAccessor.HttpContext, Substitute.For<ITempDataProvider>());
            Controller.TempData = TempData;
        }

        public async override Task When()
        {
            Result = Controller.RemoveProviderTlevelAsync(TqProviderId, false);
        }
    }
}
