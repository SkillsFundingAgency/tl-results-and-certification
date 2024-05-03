using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminAddAppealOutcomeChangeGradeSpecialismGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            CacheService.GetAsync<AdminAddAppealOutcomeChangeGradeSpecialismViewModel>(CacheKey).Returns(null as AdminAddAppealOutcomeChangeGradeSpecialismViewModel);
            AdminPostResultsLoader.GetAdminAddAppealOutcomeChangeGradeSpecialismAsync(RegistrationPathwayId, AssessmentId).Returns(ViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminAddAppealOutcomeChangeGradeSpecialismViewModel>(CacheKey);
            AdminPostResultsLoader.Received(1).GetAdminAddAppealOutcomeChangeGradeSpecialismAsync(RegistrationPathwayId, AssessmentId);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var model = Result.ShouldBeViewResult<AdminAddAppealOutcomeChangeGradeSpecialismViewModel>();
            model.Should().BeEquivalentTo(ViewModel);
        }
    }
}