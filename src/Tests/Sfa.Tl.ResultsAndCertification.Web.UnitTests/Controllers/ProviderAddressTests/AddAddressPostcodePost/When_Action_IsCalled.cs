using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddAddressPostcodePost
{
    public class When_Action_IsCalled : TestSetup
    {
        public override void Given()
        {
            ViewModel = new AddAddressPostcodeViewModel { Postcode = "AB1 2CD" };
        }

        [Fact]
        public void Then_Redirected_To_AddAddressSelect()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.AddAddressSelect);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            CacheService.Received(1).SetAsync(CacheKey,
                Arg.Is<AddAddressViewModel>(x =>
                x.AddAddressPostcode != null &&
                x.AddAddressPostcode.Postcode == ViewModel.Postcode &&
                x.AddAddressPostcode.BackLink != null &&
                x.AddAddressPostcode.BackLink.RouteName == RouteConstants.ManagePostalAddress));
        }
    }
}
