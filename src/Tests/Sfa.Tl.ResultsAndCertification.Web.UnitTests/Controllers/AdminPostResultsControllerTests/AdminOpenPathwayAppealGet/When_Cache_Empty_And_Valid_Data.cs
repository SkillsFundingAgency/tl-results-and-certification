using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminOpenPathwayAppealGet
{
    public class When_Cache_Empty_And_Valid_Data : TestSetup
    {
        private AdminOpenPathwayAppealViewModel _viewModel;

        public override void Given()
        {
            CacheService.GetAsync<AdminOpenPathwayAppealViewModel>(CacheKey).Returns(null as AdminOpenPathwayAppealViewModel);

            _viewModel = CreateViewModel();
            AdminPostResultsLoader.GetAdminOpenPathwayAppealAsync(RegistrationPathwayId, PathwayAssessmentId).Returns(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminOpenPathwayAppealViewModel>(CacheKey);
            AdminPostResultsLoader.Received(1).GetAdminOpenPathwayAppealAsync(RegistrationPathwayId, PathwayAssessmentId);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var result = Result.ShouldBeViewResult<AdminOpenPathwayAppealViewModel>();
            result.Should().BeEquivalentTo(_viewModel);
        }
    }
}