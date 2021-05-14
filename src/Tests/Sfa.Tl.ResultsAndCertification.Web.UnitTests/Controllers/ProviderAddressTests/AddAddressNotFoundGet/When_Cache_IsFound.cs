using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddAddressNotFoundGet
{
    public class When_Cache_IsFound : TestSetup
    {
        private AddAddressViewModel _cacheResult;
        private AddAddressPostcodeViewModel _cachePostcodeViewModel;

        public override void Given()
        {
            _cachePostcodeViewModel = new AddAddressPostcodeViewModel { Postcode = "AB1 2CD" };
            _cacheResult = new AddAddressViewModel { AddAddressPostcode = _cachePostcodeViewModel };
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
            viewResult.Model.Should().BeOfType(typeof(AddAddressNotFoundViewModel));

            var model = viewResult.Model as AddAddressNotFoundViewModel;
            model.Should().NotBeNull();
            model.Postcode.Should().Be(_cachePostcodeViewModel.Postcode);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AddAddressPostcode);
        }
    }
}