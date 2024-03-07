using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminChangePathwayResultClearGet
{
    public class When_Called : AdminDashboardControllerTestBase
    {
        private const int RegistrationPathwayId = 1, AssessmentId = 1;

        private IActionResult _result;

        public async override Task When()
        {
            _result = await Controller.AdminChangePathwayResultClearAsync(RegistrationPathwayId, AssessmentId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).RemoveAsync<AdminChangePathwayResultViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_AdminChangePathwayResult()
        {
            _result.ShouldBeRedirectToRouteResult(
                RouteConstants.AdminChangePathwayResult, 
                (Constants.RegistrationPathwayId, RegistrationPathwayId),
                (Constants.AssessmentId, AssessmentId));
        }
    }
}