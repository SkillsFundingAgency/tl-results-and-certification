using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminOpenSpecialismRommGet
{
    public class When_Cache_Empty_And_Valid_Data : TestSetup
    {
        private AdminOpenSpecialismRommViewModel _viewModel;

        public override void Given()
        {
            CacheService.GetAsync<AdminOpenSpecialismRommViewModel>(CacheKey).Returns(null as AdminOpenSpecialismRommViewModel);

            _viewModel = CreateViewModel();
            AdminPostResultsLoader.GetAdminOpenSpecialismRommAsync(RegistrationPathwayId, SpecialismAssessmentId).Returns(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminOpenSpecialismRommViewModel>(CacheKey);
            AdminPostResultsLoader.Received(1).GetAdminOpenSpecialismRommAsync(RegistrationPathwayId, SpecialismAssessmentId);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var result = Result.ShouldBeViewResult<AdminOpenSpecialismRommViewModel>();
            result.Should().BeEquivalentTo(_viewModel);
        }
    }
}