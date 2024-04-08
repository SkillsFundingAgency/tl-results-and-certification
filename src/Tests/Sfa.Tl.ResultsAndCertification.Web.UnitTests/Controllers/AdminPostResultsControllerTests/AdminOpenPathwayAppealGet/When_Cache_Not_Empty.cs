using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminOpenPathwayAppealGet
{
    public class When_Cache_Not_Empty : TestSetup
    {
        private AdminOpenPathwayAppealViewModel _viewModel;

        public override void Given()
        {
            _viewModel = CreateViewModel();
            CacheService.GetAsync<AdminOpenPathwayAppealViewModel>(CacheKey).Returns(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminOpenPathwayAppealViewModel>(CacheKey);
            AdminPostResultsLoader.DidNotReceive().GetAdminOpenPathwayAppealAsync(Arg.Any<int>(), Arg.Any<int>());
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var result = Result.ShouldBeViewResult<AdminOpenPathwayAppealViewModel>();
            result.Should().BeEquivalentTo(_viewModel);
        }
    }
}