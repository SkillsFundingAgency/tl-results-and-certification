using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationDateofBirthGet
{
    public class Then_On_No_Cache_Redirected_To_PageNotFound_Route : When_AddRegistrationDateofBirth_Get_Action_Is_Called
    {
        public override void Given() { }

        [Fact]
        public void Then_On_No_Cache_Redirect_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
