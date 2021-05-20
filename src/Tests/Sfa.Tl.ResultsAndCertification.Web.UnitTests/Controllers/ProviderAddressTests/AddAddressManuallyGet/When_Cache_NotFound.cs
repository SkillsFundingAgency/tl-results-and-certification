using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddAddressManuallyGet
{
    public class When_Cache_NotFound : TestSetup
    {
        private AddAddressViewModel _cacheResult;

        public override void Given()
        {
            _cacheResult = null;
            CacheService.GetAsync<AddAddressViewModel>(CacheKey).Returns(_cacheResult);
        }

        [Fact(Skip = "TODO-Ravi")]
        public void Then_Expected_Methods_Called()
        {
            CacheService.Received(1).GetAsync<AddAddressViewModel>(CacheKey);

            CacheService.Received(1).SetAsync(CacheKey,
                Arg.Is<AddAddressViewModel>
                (x => x != null &&
                x.AddAddressManual == null));
        }

        [Fact(Skip = "TODO-Ravi")]
        public void Then_Redirected_To_AddPostalAddressManual()
        {
            var route = (Result as RedirectToRouteResult);
            route.RouteName.Should().Be(RouteConstants.AddPostalAddressManual);
            route.RouteValues.Should().BeNull();
        }
    }
}
