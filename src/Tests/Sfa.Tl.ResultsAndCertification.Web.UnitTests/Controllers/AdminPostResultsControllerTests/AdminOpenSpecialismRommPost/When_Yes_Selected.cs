using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminOpenSpecialismRommPost
{
    public class When_Yes_Selected : TestSetup
    {
        private AdminOpenSpecialismRommViewModel _viewModel;

        public override void Given()
        {
            _viewModel = CreateViewModel(doYouWantToOpenRomm: true);
        }

        public async override Task When()
        {
            Result = await Controller.AdminOpenSpecialismRommAsync(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).SetAsync(CacheKey, _viewModel);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            Result.ShouldBeRedirectToRouteResult(nameof(RouteConstants.AdminOpenSpecialismRommReviewChanges));
        }
    }
}