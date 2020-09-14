using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.FindProviderPost
{
    public abstract class TestSetup : BaseTest<ProviderController>
    {
        // DI Mock objects
        protected IProviderLoader ProviderLoader;
        protected ILogger<ProviderController> Logger;

        // input, output and other mock for tes
        protected ProviderController Controller;
        protected Task<IActionResult> Result;
        protected IHttpContextAccessor HttpContextAccessor;
        protected FindProviderViewModel ViewModel;
        protected readonly long Ukprn = 1234;

        protected string ProviderName = "Lordswood School & Sixth Form Centre";
        protected int SelectedProviderId = 24;

        public override void Setup()
        {
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            ProviderLoader = Substitute.For<IProviderLoader>();
            Logger = Substitute.For<ILogger<ProviderController>>();
            Controller = new ProviderController(ProviderLoader, Logger);

            // Default value
            ViewModel = new FindProviderViewModel { Search = ProviderName, SelectedProviderId = SelectedProviderId };
            var httpContext = new ClaimsIdentityBuilder<ProviderController>(Controller)
               .Add(CustomClaimTypes.Ukprn, Ukprn.ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
        }

        public override void When()
        {
            Result = Controller.FindProviderAsync(ViewModel);
        }
    }
}
