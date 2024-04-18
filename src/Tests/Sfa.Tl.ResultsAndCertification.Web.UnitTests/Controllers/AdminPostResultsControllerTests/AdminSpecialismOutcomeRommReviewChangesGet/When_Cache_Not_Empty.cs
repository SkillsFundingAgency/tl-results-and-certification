using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminSpecilaismOutcomeRommReviewChangesGet
{
    public class When_Cache_Not_Empty : TestSetup
    {
        private AdminAddRommOutcomeChangeGradeSpecialismViewModel _viewModel;
        private AdminReviewChangesRommOutcomeSpecialismViewModel _reviewViewModel;

        public override void Given()
        {
            _viewModel = CreateViewModel();
            CacheService.GetAsync<AdminAddRommOutcomeChangeGradeSpecialismViewModel>(CacheKey).Returns(_viewModel);

            _reviewViewModel = new()
            {
                RegistrationPathwayId = _viewModel.RegistrationPathwayId,
                SpecialismAssessmentId = _viewModel.SpecialismAssessmentId,
                SpecialismResultId = _viewModel.SpecialismResultId,
                Grade = _viewModel.Grade
            };

            AdminPostResultsLoader.GetAdminReviewChangesRommOutcomeSpecialismAsync(_viewModel).Returns(_reviewViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminAddRommOutcomeChangeGradeSpecialismViewModel>(CacheKey);
            AdminPostResultsLoader.Received(1).GetAdminReviewChangesRommOutcomeSpecialismAsync(_viewModel);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var result = Result.ShouldBeViewResult<AdminReviewChangesRommOutcomeSpecialismViewModel>();
            result.Should().Be(_reviewViewModel);
        }
    }
}