using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.RemoveAssessmentEntryCoreGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            CacheService.GetAsync<AdminRemovePathwayAssessmentEntryViewModel>(CacheKey).Returns(null as AdminRemovePathwayAssessmentEntryViewModel);
            AdminDashboardLoader.GetRemovePathwayAssessmentEntryAsync(RegistrationPathwayId, AssessmentId).Returns(ViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminRemovePathwayAssessmentEntryViewModel>(CacheKey);
            AdminDashboardLoader.Received(1).GetRemovePathwayAssessmentEntryAsync(RegistrationPathwayId, AssessmentId);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var model = Result.ShouldBeViewResult<AdminRemovePathwayAssessmentEntryViewModel>();
            model.Should().BeEquivalentTo(ViewModel);
        }
    }
}