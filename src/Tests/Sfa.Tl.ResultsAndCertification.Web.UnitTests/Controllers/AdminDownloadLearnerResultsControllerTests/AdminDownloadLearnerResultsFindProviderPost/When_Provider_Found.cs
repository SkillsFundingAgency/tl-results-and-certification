using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDownloadLearnerResultsControllerTests;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDownloadLearnerResults;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminProviderControllerTests.AdminDownloadLearnerResultsFindProviderPost
{
    public class When_Provider_Found : AdminDownloadLearnerResultsControllerBaseTest
    {
        private readonly AdminDownloadLearnerResultsFindProviderViewModel _viewModel = new()
        {
            Search = "Barnsley College"
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
                }
            };


            ProviderLoader.GetProviderLookupDataAsync(_viewModel.Search, true).Returns(providers);
        }

        public async override Task When()
        {
            _result = await Controller.AdminDownloadLearnerResultsFindProviderAsync(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ProviderLoader.Received(1).GetProviderLookupDataAsync(_viewModel.Search, true);
            CacheService.Received(1).SetAsync(CacheKey, _viewModel);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            _result.ShouldBeRedirectToRouteResult(RouteConstants.AdminDownloadLearnerResultsByProvider, ("providerId", 1));
        }
    }
}