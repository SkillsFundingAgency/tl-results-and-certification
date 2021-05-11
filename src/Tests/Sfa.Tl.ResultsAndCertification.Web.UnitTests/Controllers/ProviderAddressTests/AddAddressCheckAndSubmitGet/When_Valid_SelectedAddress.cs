using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using System.Collections.Generic;
using Xunit;
using CheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.ProviderAddress.CheckAndSubmit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddAddressCheckAndSubmitGet
{
    public class When_Valid_SelectedAddress : TestSetup
    {
        private AddAddressViewModel _cacheResult;
        private AddAddressSelectViewModel _addressSelectViewModel;
        private AddressViewModel _addressSelected;

        public override void Given()
        {
            _addressSelected = new AddressViewModel
            {
                AddressLine1 = "50",
                AddressLine2 = "Street",
                Town = "Coventry",
                Postcode = "CV1 1XX",
            };

            _addressSelectViewModel = new AddAddressSelectViewModel
            {
                DepartmentName = "Finance",
                SelectedAddress = _addressSelected
            };

            _cacheResult = new AddAddressViewModel
            {
                AddAddressPostcode = new AddAddressPostcodeViewModel { Postcode = "AB1 2CD"},
                AddAddressSelect = _addressSelectViewModel,
                AddAddressManual = null
            };

            CacheService.GetAsync<AddAddressViewModel>(CacheKey).Returns(_cacheResult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            CacheService.Received(1).GetAsync<AddAddressViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(AddAddressCheckAndSubmitViewModel));

            var model = viewResult.Model as AddAddressCheckAndSubmitViewModel;
            model.Should().NotBeNull();

            model.IsValid.Should().BeTrue();

            // Department
            model.SummaryDepartment.Title.Should().Be(CheckAndSubmitContent.Summary_Department);
            model.SummaryDepartment.Value.Should().Be(_addressSelectViewModel.DepartmentName);
            model.SummaryDepartment.ActionText.Should().Be(CheckAndSubmitContent.Link_Change_Address);
            model.SummaryDepartment.RouteName.Should().BeEquivalentTo(RouteConstants.AddAddressSelect);
            model.SummaryDepartment.RouteAttributes.Should().BeNull();
            model.SummaryDepartment.NeedBorderBottomLine.Should().Be(false);

            // AddressLine1
            model.SummaryAddressLine1.Title.Should().Be(CheckAndSubmitContent.Summary_Building_And_Street);
            model.SummaryAddressLine1.Value.Should().Be(_addressSelected.AddressLine1);
            model.SummaryAddressLine1.NeedBorderBottomLine.Should().Be(false);

            // AddressLine2
            model.SummaryAddressLine2.Title.Should().BeEmpty();
            model.SummaryAddressLine2.Value.Should().Be(_addressSelected.AddressLine2);
            model.SummaryAddressLine2.NeedBorderBottomLine.Should().Be(false);

            // Town
            model.SummaryTown.Title.Should().Be(CheckAndSubmitContent.Summary_Town_Or_City);
            model.SummaryTown.Value.Should().Be(_addressSelected.Town);
            model.SummaryTown.NeedBorderBottomLine.Should().Be(false);

            // Postcode
            model.SummaryPostcode.Title.Should().Be(CheckAndSubmitContent.Summary_Postcode);
            model.SummaryPostcode.Value.Should().Be(_addressSelected.Postcode);
            model.SummaryPostcode.NeedBorderBottomLine.Should().Be(false);

            // Back link 
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AddAddressSelect);
            model.BackLink.RouteAttributes.Should().BeNull();
        }
    }
}
