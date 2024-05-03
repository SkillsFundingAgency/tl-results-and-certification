using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminAddCoreAppealOutcomeGet
{
    public class When_Cache_Empty_And_Valid_Data : TestSetup
    {
        private AdminAddCoreAppealOutcomeViewModel _viewModel;

        public override void Given()
        {
            CacheService.GetAsync<AdminAddCoreAppealOutcomeViewModel>(CacheKey).Returns(null as AdminAddCoreAppealOutcomeViewModel);

            _viewModel = CreateViewModel();
            AdminPostResultsLoader.GetAdminAddPathwayAppealOutcomeAsync(RegistrationPathwayId, PathwayAssessmentId).Returns(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminAddCoreAppealOutcomeViewModel>(CacheKey);
            AdminPostResultsLoader.Received(1).GetAdminAddPathwayAppealOutcomeAsync(RegistrationPathwayId, PathwayAssessmentId);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var result = Result.ShouldBeViewResult<AdminAddCoreAppealOutcomeViewModel>();
            result.Should().BeEquivalentTo(_viewModel);
        }
    }
}