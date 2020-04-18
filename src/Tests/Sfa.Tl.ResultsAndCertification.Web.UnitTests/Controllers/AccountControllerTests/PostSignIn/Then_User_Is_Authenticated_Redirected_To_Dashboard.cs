using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AccountControllerTests.PostSignIn
{
    public class Then_User_Is_Authenticated_Redirected_To_Dashboard : When_PostSignIn_Is_Called
    {
        public override void Given()
        {
            HttpContext.User.Identity.IsAuthenticated.Returns(true);
            HttpContext.User.Claims.Returns(new List<Claim> { new Claim(CustomClaimTypes.HasAccessToService, "true") });

            Controller.ControllerContext = new ControllerContext
            {
                HttpContext = HttpContext,
                RouteData = new Microsoft.AspNetCore.Routing.RouteData()
            };
        }

        [Fact]
        public void Then_Redirected_ToDashboard()
        {
            Result.Should().NotBeNull();
            Assert.Same((Result as RedirectToActionResult).ActionName, nameof(DashboardController.Index));
            Assert.Same((Result as RedirectToActionResult).ControllerName, Common.Helpers.Constants.DashboardController);
        }
    }
}
