using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using System.Collections.Generic;
using Xunit;
using AddAddressContent = Sfa.Tl.ResultsAndCertification.Web.Content.ProviderAddress;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddAddressSelectPost
{
    public class When_ModelState_IsInvalid : TestSetup
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

            ViewModel = new AddAddressSelectViewModel
            {
                Postcode = _cacheResult.AddAddressPostcode.Postcode
            };

            CacheService.GetAsync<AddAddressViewModel>(CacheKey).Returns(_cacheResult);
            ProviderAddressLoader.GetAddressesByPostcodeAsync(_postcodeViewModel.Postcode).Returns(_addressesMockResult);
            Controller.ModelState.AddModelError(nameof(AddAddressSelectViewModel.SelectedAddressUprn), AddAddressContent.AddAddressSelect.Validation_Select_Your_Address_From_The_List);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            ProviderAddressLoader.Received(1).GetAddressesByPostcodeAsync(_postcodeViewModel.Postcode);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            Controller.ViewData.ModelState.ContainsKey(nameof(AddAddressSelectViewModel.SelectedAddressUprn)).Should().BeTrue();
            var modelState = Controller.ViewData.ModelState[nameof(AddAddressSelectViewModel.SelectedAddressUprn)];
            modelState.Errors[0].ErrorMessage.Should().Be(AddAddressContent.AddAddressSelect.Validation_Select_Your_Address_From_The_List);

            var model = (Result as ViewResult).Model as AddAddressSelectViewModel;

            model.SelectedAddressUprn.Should().BeNull();
            model.SelectedAddress.Should().BeNull();
            model.Postcode.Should().Be(_cacheResult.AddAddressPostcode.Postcode);
            model.AddressSelectList.Should().NotBeNull();
            model.AddressSelectList.Count.Should().Be(_addressesMockResult.AddressSelectList.Count);
            model.AddressSelectList.Should().BeEquivalentTo(_addressesMockResult.AddressSelectList);
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AddAddressPostcode);
        }
    }
}
