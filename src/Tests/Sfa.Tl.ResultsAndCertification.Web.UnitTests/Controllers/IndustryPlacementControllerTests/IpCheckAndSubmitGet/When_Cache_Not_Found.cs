using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpCheckAndSubmitGet
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
            IndustryPlacementLoader.DidNotReceive().GetLearnerRecordDetailsAsync<IpCheckAndSubmitViewModel>(Arg.Any<long>(), Arg.Any<int>());
            IndustryPlacementLoader.DidNotReceive().GetTempFlexNavigationAsync(Arg.Any<int>(), Arg.Any<int>());
            IndustryPlacementLoader.DidNotReceive().GetIpSummaryDetailsListAsync(Arg.Any<IndustryPlacementViewModel>(), Arg.Any<IpTempFlexNavigation>());
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
