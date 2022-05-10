using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpTempFlexibilityUsedPost
{
    public class When_IsTempFlexibilityUsed_IsTrue : TestSetup
    {
        private IndustryPlacementViewModel _cacheResult;

        public override void Given()
        {
            // Cache object
            _cacheResult = new IndustryPlacementViewModel
            {
                IpModelViewModel = new IpModelViewModel { IpModelUsed = new IpModelUsedViewModel() }
            };
            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheResult);

            ViewModel = new IpTempFlexibilityUsedViewModel { IsTempFlexibilityUsed = true };
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).SetAsync(CacheKey, _cacheResult);
        }

        [Fact]
        public void Then_Redirected_To_Expected_Route()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.IpBlendedPlacementUsed);
        }
    }
}
