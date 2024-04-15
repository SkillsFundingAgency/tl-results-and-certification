using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminReviewSpecialismAppealChangesGet
{
    public class When_Cache_Not_Empty : TestSetup
    {
        private AdminOpenSpecialismAppealViewModel _viewModel;
        private AdminAppealSpecialismReviewChangesViewModel _reviewViewModel;

        public override void Given()
        {
            _viewModel = CreateViewModel();
            CacheService.GetAsync<AdminOpenSpecialismAppealViewModel>(CacheKey).Returns(_viewModel);

            _reviewViewModel = new()
            {
                RegistrationPathwayId = _viewModel.RegistrationPathwayId,
                SpecialismAssessmentId = _viewModel.SpecialismAssessmentId,
                SpecialismResultId = _viewModel.SpecialismResultId,
                Grade = _viewModel.Grade
            };

            AdminPostResultsLoader.GetAdminAppealSpecialismReviewChangesAsync(RegistrationPathwayId, PathwayAssessmentId).Returns(_reviewViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminOpenSpecialismAppealViewModel>(CacheKey);
            AdminPostResultsLoader.Received(1).GetAdminAppealSpecialismReviewChangesAsync(RegistrationPathwayId, PathwayAssessmentId);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var result = Result.ShouldBeViewResult<AdminAppealSpecialismReviewChangesViewModel>();
            result.Should().Be(_reviewViewModel);
        }
    }
}