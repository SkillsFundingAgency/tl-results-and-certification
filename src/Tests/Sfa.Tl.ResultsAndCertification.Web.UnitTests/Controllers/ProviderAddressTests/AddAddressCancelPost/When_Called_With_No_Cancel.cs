using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddAddressCancelPost
{
    public class When_Called_With_No_Cancel : TestSetup
    {
        public override void Given()
        {
            ViewModel = new AddAddressCancelViewModel { CancelAddAddress = false };
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            CacheService.DidNotReceive().RemoveAsync<AddAddressViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_AddAddressCheckAndSubmit()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.AddAddressCheckAndSubmit);
        }
    }
}
