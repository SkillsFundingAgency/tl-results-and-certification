using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddPostalAddressManualGet
{
    public class When_Cache_Has_No_AddAddressManual : TestSetup
    {
        private AddAddressViewModel _cacheResult;

        public override void Given()
        {
            IsFromSelectAddress = false;
            IsFromAddressMissing = true;

            _cacheResult = new AddAddressViewModel();
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
            
            model.DepartmentName.Should().BeNull();
            model.OrganisationName.Should().BeNull();
            model.AddressLine1.Should().BeNull();
            model.AddressLine1.Should().BeNull();
            model.Town.Should().BeNull();
            model.Postcode.Should().BeNull();
            model.IsFromSelectAddress.Should().Be(IsFromSelectAddress);
            model.IsFromAddressMissing.Should().Be(IsFromAddressMissing);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AddAddressPostcode);
            model.BackLink.RouteAttributes.Should().NotBeNull();
            model.BackLink.RouteAttributes.TryGetValue(Constants.ShowPostcode, out string showPostcode);
            showPostcode.Should().Be("false");
        }
    }
}
