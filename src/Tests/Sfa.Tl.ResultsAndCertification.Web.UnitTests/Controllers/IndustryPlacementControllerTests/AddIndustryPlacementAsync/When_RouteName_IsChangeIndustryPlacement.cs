using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.AddIndustryPlacementAsync
{
    public class When_RouteName_IsChangeIndustryPlacement : TestSetup
    {
        public override void Given()
        {
            ProfileId = 1;
            SetRouteAttribute(RouteConstants.ChangeIndustryPlacement);

            var ipViewModel = new IndustryPlacementViewModel { };
            IndustryPlacementLoader.GetIndustryPlacementViewModelAsync(ProviderUkprn, ProfileId).Returns(ipViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).RemoveAsync<IndustryPlacementViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_IpCompletionChange()
        {
            var route = ActualResult as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.IpCompletionChange);
            route.RouteValues.Count.Should().Be(1);
            route.RouteValues[Constants.ProfileId].Should().Be(ProfileId);
        }
    }
}