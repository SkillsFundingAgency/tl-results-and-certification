using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.RemoveAssessmentEntryCoreGet
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            AdminRemovePathwayAssessmentEntryViewModel nullModel = null;

            CacheService.GetAsync<AdminRemovePathwayAssessmentEntryViewModel>(CacheKey).Returns(nullModel);
            AdminDashboardLoader.GetRemovePathwayAssessmentEntryAsync(RegistrationPathwayId, AssessmentId).Returns(nullModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminRemovePathwayAssessmentEntryViewModel>(CacheKey);
            AdminDashboardLoader.Received(1).GetRemovePathwayAssessmentEntryAsync(RegistrationPathwayId, AssessmentId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            Result.ShouldBeRedirectPageNotFound();
        }
    }
}