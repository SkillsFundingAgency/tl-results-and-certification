using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminAddAppealOutcomeChangeGradeCoreGet
{
    public class When_Cache_Not_Empty : TestSetup
    {
        public override void Given()
        {
            CacheService.GetAsync<AdminAddAppealOutcomeChangeGradeCoreViewModel>(CacheKey).Returns(ViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminAddAppealOutcomeChangeGradeCoreViewModel>(CacheKey);
            AdminPostResultsLoader.DidNotReceive().GetAdminAddAppealOutcomeChangeGradeCoreAsync(RegistrationPathwayId, AssessmentId);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var model = Result.ShouldBeViewResult<AdminAddAppealOutcomeChangeGradeCoreViewModel>();
            model.Should().BeEquivalentTo(ViewModel);
        }
    }
}