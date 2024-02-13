using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.RemoveAssessmentEntrySpecialismGet
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            AdminRemoveSpecialismAssessmentEntryViewModel nullModel = null;

            CacheService.GetAsync<AdminRemoveSpecialismAssessmentEntryViewModel>(CacheKey).Returns(nullModel);
            AdminDashboardLoader.GetRemoveSpecialismAssessmentEntryAsync(RegistrationPathwayId, AssessmentId).Returns(nullModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminRemoveSpecialismAssessmentEntryViewModel>(CacheKey);
            AdminDashboardLoader.Received(1).GetRemoveSpecialismAssessmentEntryAsync(RegistrationPathwayId, AssessmentId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            Result.ShouldBeRedirectPageNotFound();
        }
    }
}