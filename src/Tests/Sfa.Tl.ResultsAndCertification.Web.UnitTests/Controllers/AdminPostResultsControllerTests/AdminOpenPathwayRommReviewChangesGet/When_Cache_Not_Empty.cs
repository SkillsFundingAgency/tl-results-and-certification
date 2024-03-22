using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminOpenPathwayRommReviewChangesGet
{
    public class When_Cache_Not_Empty : TestSetup
    {
        private AdminOpenPathwayRommViewModel _viewModel;
        private AdminOpenPathwayRommReviewChangesViewModel _reviewViewModel;

        public override void Given()
        {
            _viewModel = CreateViewModel();
            CacheService.GetAsync<AdminOpenPathwayRommViewModel>(CacheKey).Returns(_viewModel);

            _reviewViewModel = new()
            {
                RegistrationPathwayId = _viewModel.RegistrationPathwayId,
                PathwayAssessmentId = _viewModel.PathwayAssessmentId,
                PathwayResultId = _viewModel.PathwayResultId,
                Grade = _viewModel.Grade
            };

            AdminPostResultsLoader.GetAdminOpenPathwayRommReviewChangesAsync(_viewModel).Returns(_reviewViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminOpenPathwayRommViewModel>(CacheKey);
            AdminPostResultsLoader.Received(1).GetAdminOpenPathwayRommReviewChangesAsync(_viewModel);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var result = Result.ShouldBeViewResult<AdminOpenPathwayRommReviewChangesViewModel>();
            result.Should().Be(_reviewViewModel);
        }
    }
}