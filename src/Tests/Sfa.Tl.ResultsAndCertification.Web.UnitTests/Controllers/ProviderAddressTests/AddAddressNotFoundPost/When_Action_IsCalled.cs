using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddAddressNotFoundPost
{
    public class When_Action_IsCalled : TestSetup
    {
        public override void Given()
        {
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            CacheService.Received(1).RemoveAsync<AddAddressViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_AddAddressPostcode()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.AddAddressPostcode);
        }
    }
}