using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminAddCoreRommOutcomeAsync
{
    public class When_Cache_Empty_And_Valid_Data : TestSetup
    {
        private AdminAddCoreRommOutcomeViewModel _viewModel;

        public override void Given()
        {
            CacheService.GetAsync<AdminAddCoreRommOutcomeViewModel>(CacheKey).Returns(null as AdminAddCoreRommOutcomeViewModel);

            _viewModel = CreateViewModel();
            AdminPostResultsLoader.GetAdminAddPathwayRommOutcomeAsync(RegistrationPathwayId, PathwayAssessmentId).Returns(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminAddCoreRommOutcomeViewModel>(CacheKey);
            AdminPostResultsLoader.Received(1).GetAdminAddPathwayRommOutcomeAsync(RegistrationPathwayId, PathwayAssessmentId);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var result = Result.ShouldBeViewResult<AdminAddCoreRommOutcomeViewModel>();
            result.Should().BeEquivalentTo(_viewModel);
        }
    }
}