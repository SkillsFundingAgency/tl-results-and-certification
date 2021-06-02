using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddAddressPostcodeGet
{
    public class When_NoCache_Found_ShowPostcode_True : TestSetup
    {
        private AddAddressViewModel _cacheResult = null;

        public override void Given()
        {
            ShowPostcode = true;
            IsFromAddressMissing = true;
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
            viewResult.Model.Should().BeOfType(typeof(AddAddressPostcodeViewModel));

            var model = viewResult.Model as AddAddressPostcodeViewModel;
            model.Should().NotBeNull();
            model.Postcode.Should().BeNull();
            model.IsFromAddressMissing.Should().BeTrue();
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PostalAddressMissing);
            model.BackLink.RouteAttributes.Should().BeEmpty();
        }
    }
}
