using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDownloadLearnerResults;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDownloadLearnerResultsControllerTests.AdminDownloadLearnerResultsFindProviderClearGet
{
    public class When_Called_Redirects : AdminDownloadLearnerResultsControllerBaseTest
    {
        private IActionResult _result;

        public override async Task When()
        {
            _result = await Controller.AdminDownloadLearnerResultsFindProviderClearAsync();
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).RemoveAsync<AdminDownloadLearnerResultsFindProviderViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            _result.ShouldBeRedirectToRouteResult(RouteConstants.AdminDownloadLearnerResultsFindProvider);
        }
    }
}