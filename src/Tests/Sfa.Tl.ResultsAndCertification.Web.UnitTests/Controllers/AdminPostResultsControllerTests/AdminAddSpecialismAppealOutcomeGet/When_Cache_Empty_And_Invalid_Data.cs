using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminAddSpecialismAppealOutcomeGet
{
    public class When_Cache_Empty_And_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            AdminAddSpecialismAppealOutcomeViewModel nullModel = null;

            CacheService.GetAsync<AdminAddSpecialismAppealOutcomeViewModel>(CacheKey).Returns(nullModel);
            AdminPostResultsLoader.GetAdminAddSpecialismAppealOutcomeAsync(RegistrationPathwayId, SpecialismAssessmentId).Returns(nullModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminAddSpecialismAppealOutcomeViewModel>(CacheKey);
            AdminPostResultsLoader.Received(1).GetAdminAddSpecialismAppealOutcomeAsync(RegistrationPathwayId, SpecialismAssessmentId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            Result.ShouldBeRedirectPageNotFound();
        }
    }
}