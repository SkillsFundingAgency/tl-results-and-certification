using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminAddRommOutcomeChangeGradeCoreGet
{
    public class When_Cache_Not_Empty : TestSetup
    {
        public override void Given()
        {
            CacheService.GetAsync<AdminAddRommOutcomeChangeGradeCoreViewModel>(CacheKey).Returns(ViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminAddRommOutcomeChangeGradeCoreViewModel>(CacheKey);
            AdminPostResultsLoader.DidNotReceive().GetAdminAddRommOutcomeChangeGradeCoreAsync(RegistrationPathwayId, AssessmentId);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var model = Result.ShouldBeViewResult<AdminAddRommOutcomeChangeGradeCoreViewModel>();
            model.Should().BeEquivalentTo(ViewModel);
        }
    }
}