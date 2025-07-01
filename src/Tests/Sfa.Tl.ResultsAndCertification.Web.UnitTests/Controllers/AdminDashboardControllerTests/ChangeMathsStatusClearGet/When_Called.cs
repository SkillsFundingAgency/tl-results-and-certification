using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.SubjectsStatus;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeMathsStatusClearGet
{
    public class When_Called : AdminDashboardControllerTestBase
    {
        private IActionResult _result;
        private const int RegistrationPathwayId = 1;

        public override async Task When()
        {
            _result = await Controller.ChangeMathsStatusClearAsync(RegistrationPathwayId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).RemoveAsync<AdminChangeMathsStatusViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_AdminSearchLearnersRecords()
        {
            _result.ShouldBeRedirectToRouteResult(RouteConstants.AdminChangeMathsStatus, (Constants.RegistrationPathwayId, RegistrationPathwayId));
        }
    }
}