using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminAddSpecialismAppealOutcomeGet
{
    public class When_Cache_Empty_And_Valid_Data : TestSetup
    {
        private AdminAddSpecialismAppealOutcomeViewModel _viewModel;

        public override void Given()
        {
            CacheService.GetAsync<AdminAddSpecialismAppealOutcomeViewModel>(CacheKey).Returns(null as AdminAddSpecialismAppealOutcomeViewModel);

            _viewModel = CreateViewModel();
            AdminPostResultsLoader.GetAdminAddSpecialismAppealOutcomeAsync(RegistrationPathwayId, SpecialismAssessmentId).Returns(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminAddSpecialismAppealOutcomeViewModel>(CacheKey);
            AdminPostResultsLoader.Received(1).GetAdminAddSpecialismAppealOutcomeAsync(RegistrationPathwayId, SpecialismAssessmentId);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var result = Result.ShouldBeViewResult<AdminAddSpecialismAppealOutcomeViewModel>();
            result.Should().BeEquivalentTo(_viewModel);
        }
    }
}