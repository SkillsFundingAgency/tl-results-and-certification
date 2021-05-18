using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddAddressSelectGet
{
    public class When_NoCache_Found : TestSetup
    {
        private AddAddressViewModel _cacheResult;
        private AddAddressPostcodeViewModel _postcodeViewModel;
        private AddAddressSelectViewModel _addressesMockResult = null;

        public override void Given()
        {
            _postcodeViewModel = new AddAddressPostcodeViewModel { Postcode = "xx1 1yy" };

            _cacheResult = new AddAddressViewModel
            {
                AddAddressPostcode = _postcodeViewModel
            };

            _addressesMockResult = new AddAddressSelectViewModel
            {
                AddressSelectList = new List<SelectListItem> { new SelectListItem { Text = "Hello", Value = "236547891" } }
            };

            CacheService.GetAsync<AddAddressViewModel>(CacheKey).Returns(_cacheResult);
            ProviderAddressLoader.GetAddressesByPostcodeAsync(_postcodeViewModel.Postcode).Returns(_addressesMockResult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            CacheService.Received(1).GetAsync<AddAddressViewModel>(CacheKey);
            ProviderAddressLoader.Received(1).GetAddressesByPostcodeAsync(_postcodeViewModel.Postcode);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(AddAddressSelectViewModel));

            var model = viewResult.Model as AddAddressSelectViewModel;
            model.Should().NotBeNull();
            model.SelectedAddressUprn.Should().BeNull();
            model.SelectedAddress.Should().BeNull();
            model.DepartmentName.Should().BeNull();
            model.Postcode.Should().Be(_postcodeViewModel.Postcode);
            model.AddressSelectList.Should().NotBeNull();
            model.AddressSelectList.Count.Should().Be(_addressesMockResult.AddressSelectList.Count);
            model.AddressSelectList.Should().BeEquivalentTo(_addressesMockResult.AddressSelectList);
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AddAddressPostcode);
        }
    }
}
