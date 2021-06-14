using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddAddressManuallyGet
{
    public class When_Cache_IsFound : TestSetup
    {
        private AddAddressViewModel _cacheResult;

        public override void Given()
        {
            IsFromSelectAddress = true;
            _cacheResult = new AddAddressViewModel 
            { 
                AddAddressPostcode = new AddAddressPostcodeViewModel(), 
                AddAddressManual = new AddAddressManualViewModel(),
                AddAddressSelect = new AddAddressSelectViewModel()
            };
            CacheService.GetAsync<AddAddressViewModel>(CacheKey).Returns(_cacheResult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            CacheService.Received(1).GetAsync<AddAddressViewModel>(CacheKey);

            CacheService.Received(1).SetAsync(CacheKey,
                Arg.Is<AddAddressViewModel>
                (x => x != null &&
                x.AddAddressManual == null &&
                x.AddAddressPostcode != null &&
                x.AddAddressSelect != null));
        }

        [Fact]
        public void Then_Redirected_To_AddPostalAddressManual()
        {
            var route = (Result as RedirectToRouteResult);
            route.RouteName.Should().Be(RouteConstants.AddPostalAddressManual);
            route.RouteValues.Count.Should().Be(2);
            route.RouteValues[Constants.IsFromSelectAddress].Should().Be(true);
            route.RouteValues[Constants.IsAddressMissing].Should().Be(false);
        }
    }
}
