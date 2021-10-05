using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.QueriedDetails
{
    public class When_Tlevel_NotFound : TestSetup
    {
        public override void Given()
        {
            var mockresult = new TlevelQueriedDetailsViewModel();
            TlevelLoader.GetQueriedTlevelDetailsAsync(AoUkprn, Id)
                .Returns(mockresult);
        }

        [Fact]
        public void Then_Called_Expected_Method()
        {
            TlevelLoader.Received(1).GetQueriedTlevelDetailsAsync(AoUkprn, Id);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
