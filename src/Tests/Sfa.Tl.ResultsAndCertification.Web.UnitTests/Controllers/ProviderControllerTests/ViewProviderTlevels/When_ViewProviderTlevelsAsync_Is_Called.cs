using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.ViewProviderTlevels;
using System.Threading.Tasks;


namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.ViewProviderTlevels
{
    public abstract class When_ViewProviderTlevelsAsync_Is_Called : BaseTest<ProviderController>
    {
        // DI Mock objects
        protected IProviderLoader ProviderLoader;
        protected ILogger<ProviderController> Logger;

        // input, output and other mock for tes
        protected ProviderController Controller;
        protected Task<IActionResult> Result;
        protected IHttpContextAccessor HttpContextAccessor;
        protected ProviderViewModel ViewModel;

        // controller params
        protected int providerId = 24;
        protected bool navigation = false;

        public override void Setup()
        {
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            ProviderLoader = Substitute.For<IProviderLoader>();
            Logger = Substitute.For<ILogger<ProviderController>>();
            Controller = new ProviderController(ProviderLoader, Logger);
        }

        public override void When()
        {
            Result = Controller.ViewProviderTlevelsAsync(providerId, navigation);
        }
    }
}
