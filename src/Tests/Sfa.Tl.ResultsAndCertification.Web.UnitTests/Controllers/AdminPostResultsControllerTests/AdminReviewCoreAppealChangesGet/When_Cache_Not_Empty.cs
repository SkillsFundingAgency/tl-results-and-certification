using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminReviewCoreAppealChangesGet
{
    public class When_Cache_Not_Empty : TestSetup
    {
        private AdminOpenPathwayAppealViewModel _viewModel;
        private AdminAppealCoreReviewChangesViewModel _reviewViewModel;

        public override void Given()
        {
            _viewModel = CreateViewModel();
            CacheService.GetAsync<AdminOpenPathwayAppealViewModel>(CacheKey).Returns(_viewModel);

            _reviewViewModel = new()
            {
                RegistrationPathwayId = _viewModel.RegistrationPathwayId,
                PathwayAssessmentId = _viewModel.PathwayAssessmentId,
                PathwayResultId = _viewModel.PathwayResultId,
                Grade = _viewModel.Grade
            };

            AdminPostResultsLoader.GetAdminAppealCoreReviewChangesAsync(RegistrationPathwayId, PathwayAssessmentId).Returns(_reviewViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminOpenPathwayAppealViewModel>(CacheKey);
            AdminPostResultsLoader.Received(1).GetAdminAppealCoreReviewChangesAsync(RegistrationPathwayId, PathwayAssessmentId);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var result = Result.ShouldBeViewResult<AdminAppealCoreReviewChangesViewModel>();
            result.Should().Be(_reviewViewModel);
        }
    }
}