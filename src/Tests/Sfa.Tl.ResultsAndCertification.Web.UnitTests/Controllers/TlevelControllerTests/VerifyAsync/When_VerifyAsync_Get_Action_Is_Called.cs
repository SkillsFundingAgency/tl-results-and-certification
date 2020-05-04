using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.Session;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.VerifyAsync
{
    public abstract class When_VerifyAsync_Get_Action_Is_Called : BaseTest<TlevelController>
    {
        protected ITlevelLoader TlevelLoader;
        protected ISessionService SessionService;
        protected ILogger<TlevelController> Logger;
        protected TlevelController Controller;
        protected Task<IActionResult> Result;
        protected long ukprn;
        protected int pathwayId;

        public override void Setup()
        {
            ukprn = 9;

            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext.Returns(new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(CustomClaimTypes.Ukprn, ukprn.ToString())
                }))
            });

            TlevelLoader = Substitute.For<ITlevelLoader>();
            SessionService = Substitute.For<ISessionService>();
            Logger = Substitute.For<ILogger<TlevelController>>();
            Controller = new TlevelController(TlevelLoader, SessionService, Logger)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContextAccessor.HttpContext
                }
            };
        }

        public override void When()
        {
            Result = Controller.VerifyAsync(pathwayId, isback: false); // TODO: scenarios.
        }
    }
}
