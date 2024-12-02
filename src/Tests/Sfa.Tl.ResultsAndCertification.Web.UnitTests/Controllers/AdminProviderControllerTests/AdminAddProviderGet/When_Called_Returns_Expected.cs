using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminProvider;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminProviderControllerTests.AdminAddProviderGet
{
    public class When_ProviderDetails_Found : AdminProviderControllerBaseTest
    {
        public override void Given()
        {
        }

        private IActionResult _result;

        public override Task When()
        {
            _result = Controller.AdminAddProviderAsync();
            return Task.CompletedTask;
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var viewModel = _result.ShouldBeViewResult<AdminAddProviderViewModel>();

            viewModel.UkPrn.Should().BeNull();
            viewModel.Name.Should().BeNull();
            viewModel.DisplayName.Should().BeNull();
        }
    }
}