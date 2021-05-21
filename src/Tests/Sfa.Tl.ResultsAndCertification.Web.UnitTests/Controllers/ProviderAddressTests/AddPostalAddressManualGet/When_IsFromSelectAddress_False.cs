using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddPostalAddressManualGet
{
    public class When_IsFromSelectAddress_False : TestSetup
    {
        private AddAddressViewModel _cacheResult;
        private AddAddressPostcodeViewModel _postcodeViewModel;

        public override void Given()
        {
            IsFromSelectAddress = false;
            IsFromAddressMissing = false;

            _postcodeViewModel = new AddAddressPostcodeViewModel { Postcode = "xx1 1yy", IsFromAddressMissing = false };
            _cacheResult = new AddAddressViewModel
            {
                AddAddressPostcode = _postcodeViewModel
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
            model.IsFromSelectAddress.Should().BeFalse();
            model.IsFromAddressMissing.Should().BeFalse();
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AddAddressPostcode);
            model.BackLink.RouteAttributes.Should().NotBeNull();
            model.BackLink.RouteAttributes.TryGetValue(Constants.ShowPostcode, out string showPostcode);
            showPostcode.Should().Be("false");
        }
    }
}
