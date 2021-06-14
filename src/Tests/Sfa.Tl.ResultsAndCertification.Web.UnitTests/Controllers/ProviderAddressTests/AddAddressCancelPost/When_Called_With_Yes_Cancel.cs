using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddAddressCancelPost
{
    public class When_Called_With_Yes_Cancel : TestSetup
    {
        public override void Given()
        {
            ViewModel = new AddAddressCancelViewModel { CancelAddAddress = true };
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            CacheService.Received(1).RemoveAsync<AddAddressViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_HomePage()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.Home);
        }
    }
}
