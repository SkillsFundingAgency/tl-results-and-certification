using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.RegistrationCancelledConfirmationGet
{
    public class Then_On_No_ViewModel_Redirect_To_PageNotFound : When_RegistrationCancelledConfirmationAsync_Is_Called
    {
        public override void Given() { }

        [Fact]
        public void Then_On_No_ViewModel_ReRouted_To_PageNotFound()
        {
            // Controller
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
