using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddAddressSelectPost
{
    public class When_NoDataFound_For_SelectedUprn : TestSetup
    {
        private AddAddressViewModel _cacheResult;
        private AddAddressPostcodeViewModel _postcodeViewModel;
        private AddressViewModel _addressViewModel;
        private long _uprn;

        public override void Given()
        {
            _uprn = 236547891;
            _postcodeViewModel = new AddAddressPostcodeViewModel { Postcode = "xx1 1yy" };

            _cacheResult = new AddAddressViewModel
            {
                AddAddressPostcode = _postcodeViewModel
            };

            _addressViewModel = null;

            ViewModel = new AddAddressSelectViewModel
            {
                Postcode = _cacheResult.AddAddressPostcode.Postcode,
                SelectedAddressUprn = _uprn
            };

            CacheService.GetAsync<AddAddressViewModel>(CacheKey).Returns(_cacheResult);
            ProviderAddressLoader.GetAddressByUprnAsync(ViewModel.SelectedAddressUprn.Value).Returns(_addressViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            CacheService.Received(1).GetAsync<AddAddressViewModel>(CacheKey);
            ProviderAddressLoader.GetAddressByUprnAsync(ViewModel.SelectedAddressUprn.Value);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
