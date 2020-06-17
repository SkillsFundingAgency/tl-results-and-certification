using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.DownloadRegistrationErrors
{
    public class Then_Invalid_Id_ReRouted_To_Error_Page : When_DownloadRegistrationErrors_Is_Called
    {
        public override void Given()
        {
            Id = "Test";
        }

        [Fact]
        public void Then_On_Invalid_Id_Redirected_To_Error_Page()
        {
            var routeName = (Result.Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.Error);
        }
    }
}
