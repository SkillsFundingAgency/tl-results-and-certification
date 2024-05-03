using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminAddCoreAppealOutcomeClearGet
{
    public class When_Called : AdminPostResultsControllerTestBase
    {
        private const int RegistrationPathwayId = 1;
        private const int PathwayAssessmentId = 1;

        private IActionResult _result;

        public override async Task When()
        {
            _result = await Controller.AdminAddCoreAppealOutcomeClearAsync(RegistrationPathwayId, PathwayAssessmentId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).RemoveAsync<AdminAddCoreAppealOutcomeViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_AdminAddCoreAppealOutcome()
        {
            _result.ShouldBeRedirectToRouteResult(
                RouteConstants.AdminAddCoreAppealOutcome,
                (Constants.RegistrationPathwayId, RegistrationPathwayId),
                (Constants.AssessmentId, PathwayAssessmentId));
        }
    }
}