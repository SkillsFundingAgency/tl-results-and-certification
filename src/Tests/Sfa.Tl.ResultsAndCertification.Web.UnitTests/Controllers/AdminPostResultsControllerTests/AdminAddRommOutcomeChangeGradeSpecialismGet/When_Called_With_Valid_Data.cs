using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminAddRommOutcomeChangeGradeSpecialismGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            CacheService.GetAsync<AdminAddRommOutcomeChangeGradeSpecialismViewModel>(CacheKey).Returns(null as AdminAddRommOutcomeChangeGradeSpecialismViewModel);
            AdminPostResultsLoader.GetAdminAddRommOutcomeChangeGradeSpecialismAsync(RegistrationPathwayId, AssessmentId).Returns(ViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminAddRommOutcomeChangeGradeSpecialismViewModel>(CacheKey);
            AdminPostResultsLoader.Received(1).GetAdminAddRommOutcomeChangeGradeSpecialismAsync(RegistrationPathwayId, AssessmentId);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var model = Result.ShouldBeViewResult<AdminAddRommOutcomeChangeGradeSpecialismViewModel>();
            model.Should().BeEquivalentTo(ViewModel);
        }
    }
}