using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminAddRommOutcomeChangeGradeCoreGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            CacheService.GetAsync<AdminAddRommOutcomeChangeGradeCoreViewModel>(CacheKey).Returns(null as AdminAddRommOutcomeChangeGradeCoreViewModel);
            AdminPostResultsLoader.GetAdminAddRommOutcomeChangeGradeCoreAsync(RegistrationPathwayId, AssessmentId).Returns(ViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminAddRommOutcomeChangeGradeCoreViewModel>(CacheKey);
            AdminPostResultsLoader.Received(1).GetAdminAddRommOutcomeChangeGradeCoreAsync(RegistrationPathwayId, AssessmentId);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var model = Result.ShouldBeViewResult<AdminAddRommOutcomeChangeGradeCoreViewModel>();
            model.Should().BeEquivalentTo(ViewModel);
        }
    }
}