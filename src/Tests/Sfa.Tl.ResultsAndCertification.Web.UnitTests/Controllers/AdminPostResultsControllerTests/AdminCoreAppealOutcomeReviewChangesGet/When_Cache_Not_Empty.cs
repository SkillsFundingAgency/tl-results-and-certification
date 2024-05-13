using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminCoreAppealOutcomeReviewChangesGet
{
    public class When_Cache_Not_Empty : TestSetup
    {
        private AdminAddAppealOutcomeChangeGradeCoreViewModel _viewModel;
        private AdminReviewChangesAppealOutcomeCoreViewModel _reviewViewModel;

        public override void Given()
        {
            _viewModel = CreateViewModel();
            CacheService.GetAsync<AdminAddAppealOutcomeChangeGradeCoreViewModel>(CacheKey).Returns(_viewModel);

            _reviewViewModel = new()
            {
                RegistrationPathwayId = _viewModel.RegistrationPathwayId,
                PathwayAssessmentId = _viewModel.PathwayAssessmentId,
                PathwayResultId = _viewModel.PathwayResultId,
                Grade = _viewModel.Grade
            };

            AdminPostResultsLoader.GetAdminReviewChangesAppealOutcomeCoreAsync(_viewModel).Returns(_reviewViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminAddAppealOutcomeChangeGradeCoreViewModel>(CacheKey);
            AdminPostResultsLoader.Received(1).GetAdminReviewChangesAppealOutcomeCoreAsync(_viewModel);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var result = Result.ShouldBeViewResult<AdminReviewChangesAppealOutcomeCoreViewModel>();
            result.Should().Be(_reviewViewModel);
        }
    }
}