using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminOpenSpecialismAppealGet
{
    public class When_Cache_Not_Empty : TestSetup
    {
        private AdminOpenSpecialismAppealViewModel _viewModel;

        public override void Given()
        {
            _viewModel = CreateViewModel();
            CacheService.GetAsync<AdminOpenSpecialismAppealViewModel>(CacheKey).Returns(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminOpenSpecialismAppealViewModel>(CacheKey);
            AdminPostResultsLoader.DidNotReceive().GetAdminOpenSpecialismAppealAsync(Arg.Any<int>(), Arg.Any<int>());
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var result = Result.ShouldBeViewResult<AdminOpenSpecialismAppealViewModel>();
            result.Should().BeEquivalentTo(_viewModel);
        }
    }
}