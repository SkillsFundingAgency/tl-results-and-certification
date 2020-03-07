using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using System.Security.Claims;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DashboardControllerTests.Index
{
    public class Then_Redirected_To_ServiceAccessDenied_IsCalled : When_Index_Action_Called
    {
        public override void Given()
        {
            HttpContextAccessor.HttpContext.Returns(new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(CustomClaimTypes.HasAccessToService, "false")
                }))
            });

            Controller = new DashboardController()
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = HttpContextAccessor.HttpContext
                }
            };
        }

        [Fact]
        public void Then_Redirected_To_Route_ServiceAccessDenied()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.ServiceAccessDenied);
        }
    }
}
