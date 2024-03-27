using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminOpenSpecialismAppealClearGet
{
    public class When_Called : AdminPostResultsControllerTestBase
    {
        private const int RegistrationPathwayId = 1;
        private const int SpecialismAssessmentId = 1;

        private IActionResult _result;

        public override async Task When()
        {
            _result = await Controller.AdminOpenSpecialismAppealClearAsync(RegistrationPathwayId, SpecialismAssessmentId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).RemoveAsync<AdminOpenSpecialismAppealViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_AdminOpenPathwayRomm()
        {
            _result.ShouldBeRedirectToRouteResult(
                RouteConstants.AdminOpenSpecialismAppeal,
                (Constants.RegistrationPathwayId, RegistrationPathwayId),
                (Constants.AssessmentId, SpecialismAssessmentId));
        }
    }
}