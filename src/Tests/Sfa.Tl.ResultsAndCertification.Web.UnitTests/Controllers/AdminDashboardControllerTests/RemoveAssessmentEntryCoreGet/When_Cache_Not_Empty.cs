using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.RemoveAssessmentEntryCoreGet
{
    public class When_Cache_Not_Empty : TestSetup
    {
        public override void Given()
        {
            CacheService.GetAsync<AdminRemovePathwayAssessmentEntryViewModel>(CacheKey).Returns(ViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminRemovePathwayAssessmentEntryViewModel>(CacheKey);
            AdminDashboardLoader.DidNotReceive().GetRemovePathwayAssessmentEntryAsync(RegistrationPathwayId);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var model = Result.ShouldBeViewResult<AdminRemovePathwayAssessmentEntryViewModel>();
            model.Should().BeEquivalentTo(ViewModel);
        }
    }
}