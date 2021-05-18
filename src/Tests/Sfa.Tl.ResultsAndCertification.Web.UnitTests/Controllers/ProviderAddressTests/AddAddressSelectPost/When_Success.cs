using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddAddressSelectPost
{
    public class When_Success : TestSetup
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

            _addressViewModel = new AddressViewModel
            {
                Udprn = _uprn.ToString(),
                FormattedAddress = "5, Test road, Test town, xx1 1yy",
                DepartmentName = "Dept name",
                OrganisationName = "Org name",
                AddressLine1 = "5",
                AddressLine2 = "Test road",
                Town = "Test town",
                Postcode = "xx1 1yy"
            };

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
            ProviderAddressLoader.Received(1).GetAddressByUprnAsync(ViewModel.SelectedAddressUprn.Value);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<AddAddressViewModel>(x =>
                    x.AddAddressPostcode == _postcodeViewModel &&
                    x.AddAddressSelect == ViewModel &&
                    x.AddAddressManual == null
                    ));
        }

        [Fact]
        public void Then_Redirected_To_AddAddressCheckAndSubmit()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.AddAddressCheckAndSubmit);
        }
    }
}
