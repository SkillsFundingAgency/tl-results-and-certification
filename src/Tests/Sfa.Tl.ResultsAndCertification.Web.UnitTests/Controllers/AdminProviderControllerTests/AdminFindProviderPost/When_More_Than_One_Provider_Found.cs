using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminProviderControllerTests.AdminFindProviderPost
{
    public class When_More_Than_One_Provider_Found : AdminProviderControllerBaseTest
    {
        private readonly AdminFindProviderViewModel _viewModel = new()
        {
            Search = "Bar"
        };

        private IActionResult _result;

        public override void Given()
        {
            var providers = new ProviderLookupData[]
            {
                new()
                {
                    Id = 1,
                    DisplayName = "Barnsley College"
                },
                new()
                {
                    Id = 2,
                    DisplayName = "Barnet and Southgate College"
                }
            };

            ProviderLoader.GetProviderLookupDataAsync(_viewModel.Search, true).Returns(providers);
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