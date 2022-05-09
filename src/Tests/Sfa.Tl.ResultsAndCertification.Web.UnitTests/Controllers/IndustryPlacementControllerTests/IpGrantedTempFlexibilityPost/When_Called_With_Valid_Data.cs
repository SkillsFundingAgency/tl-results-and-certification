using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpGrantedTempFlexibilityPost
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private IndustryPlacementViewModel _cacheResult;

        public override void Given()
        {
            _cacheResult = new IndustryPlacementViewModel
            {
                IpCompletion = new IpCompletionViewModel(),
                IpModelViewModel = new IpModelViewModel { IpModelUsed = new IpModelUsedViewModel { IsIpModelUsed = false } },
                TempFlexibility = new IpTempFlexibilityViewModel { IpBlendedPlacementUsed = new IpBlendedPlacementUsedViewModel { IsBlendedPlacementUsed = false } }
            };

            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheResult);

            ViewModel = new IpGrantedTempFlexibilityViewModel
            {
                LearnerName = "Test User",
                TemporaryFlexibilities = new List<IpLookupDataViewModel>
                {
                    new IpLookupDataViewModel { Id = 1, Name = "Temp Flex 1", IsSelected = true }
                }
            };
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).SetAsync(CacheKey, _cacheResult);
        }

        [Fact]
        public void Then_Redirected_To_ExpectedRoute()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.IpCheckAndSubmit);
        }
    }
}
