using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminAddAppealOutcomeChangeGradeCoreGet
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            AdminAddAppealOutcomeChangeGradeCoreViewModel nullModel = null;

            CacheService.GetAsync<AdminAddAppealOutcomeChangeGradeCoreViewModel>(CacheKey).Returns(nullModel);
            AdminPostResultsLoader.GetAdminAddAppealOutcomeChangeGradeCoreAsync(RegistrationPathwayId, AssessmentId).Returns(nullModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminAddAppealOutcomeChangeGradeCoreViewModel>(CacheKey);
            AdminPostResultsLoader.Received(1).GetAdminAddAppealOutcomeChangeGradeCoreAsync(RegistrationPathwayId, AssessmentId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            Result.ShouldBeRedirectPageNotFound();
        }
    }
}