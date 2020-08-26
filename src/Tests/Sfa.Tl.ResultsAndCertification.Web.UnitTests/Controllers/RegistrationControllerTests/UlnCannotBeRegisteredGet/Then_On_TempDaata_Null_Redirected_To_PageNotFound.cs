using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.UlnCannotBeRegisteredGet
{
    public class Then_On_TempDaata_Null_Redirected_To_PageNotFound : When_UlnCannotBeRegistered_Action_Is_Called
    {
        public override void Given() { }

        [Fact]
        public void Then_On_No_TempData_ReRouted_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
