using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddAddressManuallyGet
{
    public class When_Cache_NotFound : TestSetup
    {
        private AddAddressViewModel _cacheResult;

        public override void Given()
        {
            IsFromAddressMissing = true;
            IsFromSelectAddress = true;

            _cacheResult = null;
            CacheService.GetAsync<AddAddressViewModel>(CacheKey).Returns(_cacheResult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            CacheService.Received(1).GetAsync<AddAddressViewModel>(CacheKey);
            CacheService.DidNotReceive().SetAsync(Arg.Any<string>(), Arg.Any<AddAddressViewModel>());
        }

        [Fact]
        public void Then_Redirected_To_AddPostalAddressManual()
        {
            var route = (Result as RedirectToRouteResult);
            route.RouteName.Should().Be(RouteConstants.AddPostalAddressManual);
            route.RouteValues.Should().HaveCount(2);
            route.RouteValues[Constants.IsFromSelectAddress].Should().Be(true);
            route.RouteValues[Constants.IsAddressMissing].Should().Be(true);
        }
    }
}
