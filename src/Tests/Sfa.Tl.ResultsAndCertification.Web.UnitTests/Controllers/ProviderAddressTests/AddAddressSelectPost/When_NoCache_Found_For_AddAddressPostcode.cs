using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddAddressSelectPost
{
    public class When_NoCache_Found_For_AddAddressPostcode : TestSetup
    {
        private AddAddressViewModel _cacheResult;
        public override void Given()
        {
            ViewModel = new AddAddressSelectViewModel
            {
                SelectedAddressUprn = 123456789,
                Postcode = "xx1 1yy"
            };

            _cacheResult = new AddAddressViewModel();
            CacheService.GetAsync<AddAddressViewModel>(CacheKey).Returns(_cacheResult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            CacheService.Received(1).GetAsync<AddAddressViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
