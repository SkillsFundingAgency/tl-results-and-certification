using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminSpecialismOutcomeAppealReviewChangesGet
{
    public class When_Cache_Not_Empty : TestSetup
    {
        private AdminAddAppealOutcomeChangeGradeSpecialismViewModel _viewModel;
        private AdminReviewChangesAppealOutcomeSpecialismViewModel _reviewViewModel;

        public override void Given()
        {
            _viewModel = CreateViewModel();
            CacheService.GetAsync<AdminAddAppealOutcomeChangeGradeSpecialismViewModel>(CacheKey).Returns(_viewModel);

            _reviewViewModel = new()
            {
                RegistrationPathwayId = _viewModel.RegistrationPathwayId,
                SpecialismAssessmentId = _viewModel.SpecialismAssessmentId,
                SpecialismResultId = _viewModel.SpecialismResultId,
                Grade = _viewModel.Grade
            };

            AdminPostResultsLoader.GetAdminReviewChangesAppealOutcomeSpecialismAsync(_viewModel).Returns(_reviewViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminAddAppealOutcomeChangeGradeSpecialismViewModel>(CacheKey);
            AdminPostResultsLoader.Received(1).GetAdminReviewChangesAppealOutcomeSpecialismAsync(_viewModel);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var result = Result.ShouldBeViewResult<AdminReviewChangesAppealOutcomeSpecialismViewModel>();
            result.Should().Be(_reviewViewModel);
        }
    }
}