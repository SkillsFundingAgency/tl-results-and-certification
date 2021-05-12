using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddPostalAddressManualGet
{
    public class When_Cache_NotFound : TestSetup
    {
        private AddAddressViewModel _cacheResult;

        public override void Given()
        {
            _cacheResult = null;
            CacheService.GetAsync<AddAddressViewModel>(CacheKey).Returns(_cacheResult);
        }


        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
