using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Linq;
using System.Security.Claims;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ErrorControllerTests.ServiceAccessDenied
{
    public class When_User_Not_In_FreezePeriod : TestSetup
    {
        public override void Given()
        {
            HttpContext.User.Identity.IsAuthenticated.Returns(true);
            HttpContext.User.Claims.Returns(Enumerable.Empty<Claim>());

            Controller.ControllerContext = new ControllerContext
            {
                HttpContext = HttpContext,
                RouteData = new Microsoft.AspNetCore.Routing.RouteData()
            };
        }

        [Fact]
        public void Then_Redirected_To_ServiceUnavailable()
        {
            Result.Should().NotBeNull().And.BeOfType<ViewResult>();
        }
    }
}