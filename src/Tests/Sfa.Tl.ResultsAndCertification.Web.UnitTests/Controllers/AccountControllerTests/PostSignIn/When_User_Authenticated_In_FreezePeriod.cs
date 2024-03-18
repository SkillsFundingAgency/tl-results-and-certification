using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using System.Security.Claims;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AccountControllerTests.PostSignIn
{
    public class When_User_Authenticated_In_FreezePeriod : TestSetup
    {
        public override void Given()
        {
            HttpContext.User.Identity.IsAuthenticated.Returns(true);
            HttpContext.User.Claims.Returns(new Claim[]
            {
                new Claim(CustomClaimTypes.HasAccessToService, "false"),
                new Claim(CustomClaimTypes.InFreezePeriod, "true")
            });

            Controller.ControllerContext = new ControllerContext
            {
                HttpContext = HttpContext,
                RouteData = new Microsoft.AspNetCore.Routing.RouteData()
            };
        }

        [Fact]
        public void Then_Redirected_To_ServiceUnavailable()
        {
            Result.ShouldBeRedirectToActionResult(RouteConstants.ServiceUnavailable);
        }
    }
}