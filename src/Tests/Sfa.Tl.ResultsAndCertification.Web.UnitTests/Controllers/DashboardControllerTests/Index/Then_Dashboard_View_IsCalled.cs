using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using System.Security.Claims;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DashboardControllerTests.Index
{
    public class Then_Dashboard_View_IsCalled : When_Index_Action_Called
    {
        public override void Given()
        {
            HttpContextAccessor.HttpContext.Returns(new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(CustomClaimTypes.HasAccessToService, "true")
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
        public void Then_Dashboard_View_Page_IsCalled()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType<ViewResult>();
            var viewResult = Result as ViewResult;
            viewResult.Should().NotBeNull();
        }
    }
}
