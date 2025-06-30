using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LevelTwoResults;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeEnglishStatusClearGet
{
    public class When_Called : AdminDashboardControllerTestBase
    {
        private IActionResult _result;
        private const int RegistrationPathwayId = 1;

        public override async Task When()
        {
            _result = await Controller.ChangeEnglishStatusClearAsync(RegistrationPathwayId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).RemoveAsync<AdminChangeEnglishResultsViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_AdminSearchLearnersRecords()
        {
            _result.ShouldBeRedirectToRouteResult(RouteConstants.AdminChangeEnglishStatus, (Constants.RegistrationPathwayId, RegistrationPathwayId));
        }
    }
}