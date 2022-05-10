using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpBlendedPlacementUsedPost
{
    public class When_Cache_Not_Found : TestSetup
    {
        private readonly IndustryPlacementViewModel _cacheResult = null;

        public override void Given()
        {
            // Cache object
            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            IndustryPlacementLoader.DidNotReceive().GetTempFlexNavigationAsync(Arg.Any<int>(), Arg.Any<int>());
            IndustryPlacementLoader.DidNotReceive().TransformIpCompletionDetailsTo<IpBlendedPlacementUsedViewModel>(Arg.Any<IpCompletionViewModel>());
            CacheService.DidNotReceive().SetAsync(CacheKey, _cacheResult);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
