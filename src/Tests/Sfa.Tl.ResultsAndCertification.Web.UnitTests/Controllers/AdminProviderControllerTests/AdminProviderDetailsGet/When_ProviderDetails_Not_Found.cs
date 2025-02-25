using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminProvider;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminProviderControllerTests.AdminProviderDetailsGet
{
    public class When_ProviderDetails_Not_Found : AdminProviderControllerBaseTest
    {
        private const int ProviderId = 1;

        public override void Given()
        {
            AdminProviderLoader.GetProviderDetailsViewModel(ProviderId).Returns(null as AdminProviderDetailsViewModel);
        }

        private IActionResult _result;

        public async override Task When()
        {
            _result = await Controller.AdminProviderDetailsAsync(ProviderId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminProviderLoader.Received(1).GetProviderDetailsViewModel(ProviderId);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            _result.ShouldBeRedirectToProblemWithService();
        }
    }
}