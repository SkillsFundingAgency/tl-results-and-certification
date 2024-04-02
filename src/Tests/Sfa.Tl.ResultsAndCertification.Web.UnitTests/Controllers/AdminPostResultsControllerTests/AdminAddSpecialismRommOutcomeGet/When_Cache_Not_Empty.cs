using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminAddSpecialismRommOutcomeGet
{
    public class When_Cache_Not_Empty : TestSetup
    {
        private AdminAddSpecialismRommOutcomeViewModel _viewModel;

        public override void Given()
        {
            _viewModel = CreateViewModel();
            CacheService.GetAsync<AdminAddSpecialismRommOutcomeViewModel>(CacheKey).Returns(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminAddSpecialismRommOutcomeViewModel>(CacheKey);
            AdminPostResultsLoader.DidNotReceive().GetAdminAddSpecialismRommOutcomeAsync(Arg.Any<int>(), Arg.Any<int>());
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var result = Result.ShouldBeViewResult<AdminAddSpecialismRommOutcomeViewModel>();
            result.Should().BeEquivalentTo(_viewModel);
        }
    }
}