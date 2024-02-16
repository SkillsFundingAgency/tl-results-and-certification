using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminAddSpecialismResultClearGet
{
    public class When_Called : AdminDashboardControllerTestBase
    {
        private const int RegistrationPathwayId = 1, AssessmentId = 1;

        private IActionResult _result;

        public async override Task When()
        {
            _result = await Controller.AdminAddSpecialismResultClearAsync(RegistrationPathwayId, AssessmentId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).RemoveAsync<AdminAddSpecialismResultViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_AdminAddSpecialismResult()
        {
            _result.ShouldBeRedirectToRouteResult(
                RouteConstants.AdminAddSpecialismResult,
                (Constants.RegistrationPathwayId, RegistrationPathwayId),
                (Constants.AssessmentId, AssessmentId));
        }
    }
}