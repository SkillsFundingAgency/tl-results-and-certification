using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminOpenSpecialismAppealGet
{
    public class When_Cache_Empty_And_Valid_Data : TestSetup
    {
        private AdminOpenSpecialismAppealViewModel _viewModel;

        public override void Given()
        {
            CacheService.GetAsync<AdminOpenSpecialismAppealViewModel>(CacheKey).Returns(null as AdminOpenSpecialismAppealViewModel);

            _viewModel = CreateViewModel();
            AdminPostResultsLoader.GetAdminOpenSpecialismAppealAsync(RegistrationPathwayId, SpecialismAssessmentId).Returns(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminOpenSpecialismAppealViewModel>(CacheKey);
            AdminPostResultsLoader.Received(1).GetAdminOpenSpecialismAppealAsync(RegistrationPathwayId, SpecialismAssessmentId);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var result = Result.ShouldBeViewResult<AdminOpenSpecialismAppealViewModel>();
            result.Should().BeEquivalentTo(_viewModel);
        }
    }
}