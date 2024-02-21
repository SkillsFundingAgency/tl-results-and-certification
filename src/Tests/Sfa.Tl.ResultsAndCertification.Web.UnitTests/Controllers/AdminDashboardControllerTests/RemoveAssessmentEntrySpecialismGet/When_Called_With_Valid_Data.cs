using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.RemoveAssessmentEntrySpecialismGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            CacheService.GetAsync<AdminRemoveSpecialismAssessmentEntryViewModel>(CacheKey).Returns(null as AdminRemoveSpecialismAssessmentEntryViewModel);
            AdminDashboardLoader.GetRemoveSpecialismAssessmentEntryAsync(RegistrationPathwayId, AssessmentId).Returns(ViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminRemoveSpecialismAssessmentEntryViewModel>(CacheKey);
            AdminDashboardLoader.Received(1).GetRemoveSpecialismAssessmentEntryAsync(RegistrationPathwayId, AssessmentId);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var model = Result.ShouldBeViewResult<AdminRemoveSpecialismAssessmentEntryViewModel>();
            model.Should().BeEquivalentTo(ViewModel);
        }
    }
}