using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminAddCoreAppealOutcomeGet
{
    public class When_Cache_Not_Empty : TestSetup
    {
        private AdminAddCoreAppealOutcomeViewModel _viewModel;

        public override void Given()
        {
            _viewModel = CreateViewModel();
            CacheService.GetAsync<AdminAddCoreAppealOutcomeViewModel>(CacheKey).Returns(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminAddCoreAppealOutcomeViewModel>(CacheKey);
            AdminPostResultsLoader.DidNotReceive().GetAdminAddPathwayAppealOutcomeAsync(Arg.Any<int>(), Arg.Any<int>());
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var result = Result.ShouldBeViewResult<AdminAddCoreAppealOutcomeViewModel>();
            result.Should().BeEquivalentTo(_viewModel);
        }
    }
}