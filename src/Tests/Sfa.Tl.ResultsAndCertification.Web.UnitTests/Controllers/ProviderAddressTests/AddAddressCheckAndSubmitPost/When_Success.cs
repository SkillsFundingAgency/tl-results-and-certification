using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddAddressCheckAndSubmitPost
{
    public class When_Success : TestSetup
    {
        public override void Given()
        {
            AddAddressPostcode = new AddAddressPostcodeViewModel { Postcode = "xx1 1yy" };
            AddAddressSelect = new AddAddressSelectViewModel { SelectedAddressUprn = 123456789, SelectedAddress = new AddressViewModel { OrganisationName = "Org name", AddressLine1 = "Line1", AddressLine2 = "Line2", Town = "town", Postcode = "xx1 1yy" } };

            AddAddressViewModel = new AddAddressViewModel
            {
                AddAddressPostcode = AddAddressPostcode,
                AddAddressSelect = AddAddressSelect
            };

            CacheService.GetAsync<AddAddressViewModel>(CacheKey).Returns(AddAddressViewModel);
            ProviderAddressLoader.AddAddressAsync(ProviderUkprn, AddAddressViewModel).Returns(true);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            ProviderAddressLoader.Received(1).AddAddressAsync(ProviderUkprn, AddAddressViewModel);
            CacheService.Received(1).RemoveAsync<AddAddressViewModel>(CacheKey);
            CacheService.Received(1).SetAsync(string.Concat(CacheKey, Constants.AddAddressConfirmation),
                Arg.Is<bool>(x => x == true), CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Redirected_To_AddAddressConfirmation()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.AddAddressConfirmation);
        }
    }
}
