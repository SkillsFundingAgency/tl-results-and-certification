using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminOpenPathwayRommGet
{
    public class When_Cache_Empty_And_Valid_Data : TestSetup
    {
        private AdminOpenPathwayRommViewModel _viewModel;

        public override void Given()
        {
            CacheService.GetAsync<AdminOpenPathwayRommViewModel>(CacheKey).Returns(null as AdminOpenPathwayRommViewModel);

            _viewModel = CreateViewModel();
            AdminPostResultsLoader.GetAdminOpenPathwayRommAsync(RegistrationPathwayId, PathwayAssessmentId).Returns(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminOpenPathwayRommViewModel>(CacheKey);
            AdminPostResultsLoader.Received(1).GetAdminOpenPathwayRommAsync(RegistrationPathwayId, PathwayAssessmentId);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var result = Result.ShouldBeViewResult<AdminOpenPathwayRommViewModel>();
            result.Should().BeEquivalentTo(_viewModel);
        }
    }
}