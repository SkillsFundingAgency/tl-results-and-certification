using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDownloadLearnerResultsControllerTests;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDownloadLearnerResults;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminProviderControllerTests.AdminDownloadLearnerResultsByProviderGet
{
    public class When_ProviderDetails_Not_Found : AdminDownloadLearnerResultsControllerBaseTest
    {
        private const int ProviderId = 1;

        public override void Given()
        {
            AdminDownloadLearnerResultsLoader
                .GetDownloadLearnerResultsByProviderViewModel(ProviderId)
                .Returns(null as AdminDownloadLearnerResultsByProviderViewModel);
        }

        private IActionResult _result;

        public async override Task When()
        {
            _result = await Controller.AdminDownloadLearnerResultsByProviderAsync(ProviderId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDownloadLearnerResultsLoader.Received(1).GetDownloadLearnerResultsByProviderViewModel(ProviderId);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            _result.ShouldBeRedirectToProblemWithService();
        }
    }
}