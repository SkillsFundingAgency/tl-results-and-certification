using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddAddressCheckAndSubmitPost
{
    public class When_Failed : TestSetup
    {
        public override void Given()
        {
            AddAddressPostcode = new AddAddressPostcodeViewModel { Postcode = "xx1 1yy" };
            AddAddressSelect = new AddAddressSelectViewModel { SelectedAddressUprn = 123456789, SelectedAddress = new AddressViewModel { AddressLine1 = "Line1", AddressLine2 = "Line2", Town = "town", Postcode = "xx1 1yy" } };

            AddAddressViewModel = new AddAddressViewModel
            {
                AddAddressPostcode = AddAddressPostcode,
                AddAddressSelect = AddAddressSelect
            };

            CacheService.GetAsync<AddAddressViewModel>(CacheKey).Returns(AddAddressViewModel);
            ProviderAddressLoader.AddAddressAsync(ProviderUkprn, AddAddressViewModel).Returns(false);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            ProviderAddressLoader.Received(1).AddAddressAsync(ProviderUkprn, AddAddressViewModel);
            CacheService.DidNotReceive().RemoveAsync<AddAddressViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_Error()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            var routeValue = (Result as RedirectToRouteResult).RouteValues["StatusCode"];
            routeName.Should().Be(RouteConstants.Error);
            routeValue.Should().Be(500);
        }
    }
}
