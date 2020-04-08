using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using System.Collections.Generic;
using System.Security.Claims;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AccountControllerTests.PostSignIn
{
    public abstract class When_PostSignIn_Is_Called : BaseTest<AccountController>
    {
        protected AccountController Controller;
        protected IActionResult Result;
        protected ILogger<AccountController> Logger;
        protected IHttpContextAccessor HttpContextAccessor;

        public ResultsAndCertificationConfiguration Configuration { get; set; }

        public override void Setup()
        {
            var identity = Substitute.For<ClaimsIdentity>();
            identity.IsAuthenticated.Returns(true);
            identity.Name.Returns("AuthUser");

            var claimsPrincipal = Substitute.For<ClaimsPrincipal>();
            claimsPrincipal.Identity.Returns(identity);
            claimsPrincipal.Claims.Returns(new List<Claim> { new Claim(CustomClaimTypes.HasAccessToService, "true") });

            var httpContext = Substitute.For<HttpContext>();
            httpContext.User.Returns(claimsPrincipal);

            Logger = Substitute.For<ILogger<AccountController>>();
            Controller = new AccountController(Logger, Configuration)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                    RouteData  = new Microsoft.AspNetCore.Routing.RouteData()
                }
            };
        }

        public override void When()
        {
            Result = Controller.PostSignIn();
        }
    }
}
