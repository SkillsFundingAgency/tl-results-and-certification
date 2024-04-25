using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminAddAppealOutcomeChangeGradeSpecialismGet
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            AdminAddAppealOutcomeChangeGradeSpecialismViewModel nullModel = null;

            CacheService.GetAsync<AdminAddAppealOutcomeChangeGradeSpecialismViewModel>(CacheKey).Returns(nullModel);
            AdminPostResultsLoader.GetAdminAddAppealOutcomeChangeGradeSpecialismAsync(RegistrationPathwayId, AssessmentId).Returns(nullModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminAddAppealOutcomeChangeGradeSpecialismViewModel>(CacheKey);
            AdminPostResultsLoader.Received(1).GetAdminAddAppealOutcomeChangeGradeSpecialismAsync(RegistrationPathwayId, AssessmentId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            Result.ShouldBeRedirectPageNotFound();
        }
    }
}