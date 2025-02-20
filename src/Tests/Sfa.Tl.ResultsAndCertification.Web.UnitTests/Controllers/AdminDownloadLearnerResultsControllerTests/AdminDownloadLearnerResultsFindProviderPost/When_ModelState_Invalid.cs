using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDownloadLearnerResults;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDownloadLearnerResults;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDownloadLearnerResultsControllerTests.AdminDownloadLearnerResultsFindProviderPost
{
    public class When_ModelState_Invalid : AdminDownloadLearnerResultsControllerBaseTest
    {
        private readonly AdminDownloadLearnerResultsFindProviderViewModel _viewModel = new()
        {
            Search = string.Empty
        };

        private IActionResult _result;

        public override void Given()
        {
            Controller.ModelState.AddModelError("Search", AdminDownloadLearnerResultsFindProvider.ProviderName_Required_Validation_Message);
        }

        public override async Task When()
        {
            _result = await Controller.AdminDownloadLearnerResultsFindProviderAsync(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ProviderLoader.DidNotReceive().GetProviderLookupDataAsync(Arg.Any<string>(), Arg.Any<bool>());
            CacheService.DidNotReceive().SetAsync(CacheKey, _viewModel);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            Controller.ModelState.ErrorCount.Should().Be(1);
            Controller.ModelState.Should().ContainKey("Search");

            var resultViewModel = _result.ShouldBeViewResult<AdminDownloadLearnerResultsFindProviderViewModel>();
            resultViewModel.Should().Be(_viewModel);
        }
    }
}