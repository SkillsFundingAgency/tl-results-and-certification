using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Xunit;
using CheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.ProviderAddress.CheckAndSubmit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddAddressCheckAndSubmitGet
{
    public class When_Valid_ManualAddress : TestSetup
    {
        private AddAddressViewModel _cacheResult;
        private AddAddressManualViewModel _manualAddress;

        public override void Given()
        {
            _manualAddress = new AddAddressManualViewModel
            {
                DepartmentName = "Finance",
                OrganisationName = "Org name",
                AddressLine1 = "50",
                AddressLine2 = "Street",
                Town = "Coventry",
                Postcode = "CV1 1XX",
                IsFromSelectAddress = true
            };

            _cacheResult = new AddAddressViewModel
            {
                AddAddressPostcode = null,
                AddAddressSelect = null,
                AddAddressManual = _manualAddress
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
            model.SummaryDepartment.Value.Should().Be(_manualAddress.DepartmentName);
            model.SummaryDepartment.ActionText.Should().Be(CheckAndSubmitContent.Link_Change_Address);
            model.SummaryDepartment.RouteName.Should().BeEquivalentTo(RouteConstants.AddPostalAddressManual);
            model.SummaryDepartment.RouteAttributes.Should().BeNull();

            // Organisation Name
            model.SummaryOrganisationName.Title.Should().Be(CheckAndSubmitContent.Summary_OrganisationName);
            model.SummaryOrganisationName.Value.Should().Be(_manualAddress.OrganisationName);
            model.SummaryOrganisationName.NeedBorderBottomLine.Should().Be(false);

            // AddressLine1
            model.SummaryAddressLine1.Title.Should().Be(CheckAndSubmitContent.Summary_Building_And_Street);
            model.SummaryAddressLine1.Value.Should().Be(_manualAddress.AddressLine1);
            model.SummaryAddressLine1.NeedBorderBottomLine.Should().Be(false);

            // AddressLine2
            model.SummaryAddressLine2.Title.Should().BeEmpty();
            model.SummaryAddressLine2.Value.Should().Be(_manualAddress.AddressLine2);
            model.SummaryAddressLine2.NeedBorderBottomLine.Should().Be(false);

            // Town
            model.SummaryTown.Title.Should().Be(CheckAndSubmitContent.Summary_Town_Or_City);
            model.SummaryTown.Value.Should().Be(_manualAddress.Town);
            model.SummaryTown.NeedBorderBottomLine.Should().Be(false);

            // Postcode
            model.SummaryPostcode.Title.Should().Be(CheckAndSubmitContent.Summary_Postcode);
            model.SummaryPostcode.Value.Should().Be(_manualAddress.Postcode);
            model.SummaryPostcode.NeedBorderBottomLine.Should().Be(false);

            // Back link 
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AddPostalAddressManual);
            model.BackLink.RouteAttributes.Count.Should().Be(0);
        }
    }
}
