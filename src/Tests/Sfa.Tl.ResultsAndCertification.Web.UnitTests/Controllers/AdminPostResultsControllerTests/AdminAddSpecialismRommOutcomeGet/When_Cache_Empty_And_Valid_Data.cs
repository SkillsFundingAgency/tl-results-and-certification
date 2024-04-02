using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminAddSpecialismRommOutcomeGet
{
    public class When_Cache_Empty_And_Valid_Data : TestSetup
    {
        private AdminAddSpecialismRommOutcomeViewModel _viewModel;

        public override void Given()
        {
            CacheService.GetAsync<AdminAddSpecialismRommOutcomeViewModel>(CacheKey).Returns(null as AdminAddSpecialismRommOutcomeViewModel);

            _viewModel = CreateViewModel();
            AdminPostResultsLoader.GetAdminAddSpecialismRommOutcomeAsync(RegistrationPathwayId, SpecialismAssessmentId).Returns(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminAddSpecialismRommOutcomeViewModel>(CacheKey);
            AdminPostResultsLoader.Received(1).GetAdminAddSpecialismRommOutcomeAsync(RegistrationPathwayId, SpecialismAssessmentId);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var result = Result.ShouldBeViewResult<AdminAddSpecialismRommOutcomeViewModel>();
            result.Should().BeEquivalentTo(_viewModel);
        }
    }
}