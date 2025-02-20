using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDownloadLearnerResults;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDownloadLearnerResultsControllerTests.AdminDownloadLearnerResultsFindProviderGet
{
    public class When_Cache_Empty : AdminDownloadLearnerResultsControllerBaseTest
    {
        private readonly AdminDownloadLearnerResultsFindProviderViewModel _viewModel = new();
        private IActionResult _result;

        public override void Given()
        {
            CacheService.GetAsync<AdminDownloadLearnerResultsFindProviderViewModel>(CacheKey)
                .Returns(null as AdminDownloadLearnerResultsFindProviderViewModel);

        }

        public override async Task When()
        {
            _result = await Controller.AdminDownloadLearnerResultsFindProviderAsync();
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminDownloadLearnerResultsFindProviderViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var model = _result.ShouldBeViewResult<AdminDownloadLearnerResultsFindProviderViewModel>();
            model.Should().BeEquivalentTo(_viewModel);
        }
    }
}
