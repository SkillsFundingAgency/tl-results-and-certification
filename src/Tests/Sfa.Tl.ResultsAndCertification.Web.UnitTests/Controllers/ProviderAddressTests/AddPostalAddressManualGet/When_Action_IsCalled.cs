using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddPostalAddressManualGet
{
    public class When_Action_IsCalled : TestSetup
    {
        public override void Given() { }

        [Fact(Skip = "Todo")]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as AddPostalAddressManualViewModel;

            model.Should().NotBeNull();
            model.Department.Should().BeNull();
            model.AddressLine1.Should().BeNull();
            model.AddressLine2.Should().BeNull();
            model.Town.Should().BeNull();
            model.Postcode.Should().BeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AddAddressPostcode);
        }
    }
}
