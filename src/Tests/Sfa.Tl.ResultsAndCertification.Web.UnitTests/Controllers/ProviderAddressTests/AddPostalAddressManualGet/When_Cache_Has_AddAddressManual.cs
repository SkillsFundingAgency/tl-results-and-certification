using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddPostalAddressManualGet
{
    public class When_Cache_Has_AddAddressManual : TestSetup
    {
        private AddAddressViewModel _cacheResult;
        private AddAddressManualViewModel _addressManual;

        public override void Given()
        {
            IsFromSelectAddress = true;
            
            _addressManual = new AddAddressManualViewModel
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
                AddAddressManual = _addressManual
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
            viewResult.Model.Should().BeOfType(typeof(AddAddressManualViewModel));

            var model = viewResult.Model as AddAddressManualViewModel;
            model.Should().NotBeNull();

            model.DepartmentName.Should().Be(_addressManual.DepartmentName);
            model.OrganisationName.Should().Be(_addressManual.OrganisationName);
            model.AddressLine1.Should().Be(_addressManual.AddressLine1);
            model.AddressLine1.Should().Be(_addressManual.AddressLine1);
            model.Town.Should().Be(_addressManual.Town);
            model.Postcode.Should().Be(_addressManual.Postcode);
            model.IsFromSelectAddress.Should().Be(IsFromSelectAddress);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AddAddressSelect);
            model.BackLink.RouteAttributes.Should().BeNull();
        }
    }
}
