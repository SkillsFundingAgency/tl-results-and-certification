using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminOpenPathwayRommClearGet
{
    public class When_Called : AdminPostResultsControllerTestBase
    {
        private const int RegistrationPathwayId = 1;
        private const int PathwayAssessmentId = 1;

        private IActionResult _result;

        public override async Task When()
        {
            _result = await Controller.AdminOpenPathwayRommClearAsync(RegistrationPathwayId, PathwayAssessmentId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).RemoveAsync<AdminOpenPathwayRommViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_AdminOpenPathwayRomm()
        {
            _result.ShouldBeRedirectToRouteResult(
                RouteConstants.AdminOpenPathwayRomm,
                (Constants.RegistrationPathwayId, RegistrationPathwayId),
                (Constants.AssessmentId, PathwayAssessmentId));
        }
    }
}