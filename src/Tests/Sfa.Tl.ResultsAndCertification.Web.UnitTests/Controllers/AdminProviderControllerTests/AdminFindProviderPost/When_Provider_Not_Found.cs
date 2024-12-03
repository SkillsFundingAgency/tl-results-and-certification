using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminProviderControllerTests.AdminFindProviderPost
{
    public class When_Provider_Not_Found : AdminProviderControllerBaseTest
    {
        private readonly AdminFindProviderViewModel _viewModel = new()
        {
            Search = "Barnsley College"
        };

        private IActionResult _result;

        public override void Given()
        {
            ProviderLoader.GetProviderLookupDataAsync(_viewModel.Search, true).Returns(Enumerable.Empty<ProviderLookupData>());
        }

        public async override Task When()
        {
            _result = await Controller.AdminFindProviderAsync(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ProviderLoader.Received(1).GetProviderLookupDataAsync(_viewModel.Search, true);
            CacheService.DidNotReceive().SetAsync(CacheKey, _viewModel);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            Controller.ModelState.ErrorCount.Should().Be(1);
            Controller.ModelState.Should().ContainKey("Search");

            var resultViewModel = _result.ShouldBeViewResult<AdminFindProviderViewModel>();
            resultViewModel.Should().Be(_viewModel);
        }
    }
}