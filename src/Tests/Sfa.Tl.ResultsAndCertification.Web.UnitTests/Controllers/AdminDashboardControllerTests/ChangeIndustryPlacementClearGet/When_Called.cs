using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeIndustryPlacementClearGet
{
    public class When_Called : AdminDashboardControllerTestBase
    {
        private IActionResult _result;
        private const int RegistrationPathwayId = 1250;

        public override async Task When()
        {
            _result = await Controller.ChangeIndustryPlacementClearAsync(RegistrationPathwayId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).RemoveAsync<AdminChangeIpViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_AdminSearchLearnersRecords()
        {
            _result.ShouldBeRedirectToRouteResult(RouteConstants.AdminChangeIndustryPlacement, (Constants.RegistrationPathwayId, RegistrationPathwayId));
        }
    }
}