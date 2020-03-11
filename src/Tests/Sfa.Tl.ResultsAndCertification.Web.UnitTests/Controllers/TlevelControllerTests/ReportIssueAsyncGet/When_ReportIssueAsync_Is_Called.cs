using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ReportIssueAsyncGet
{
    public abstract class When_ReportIssueAsync_Is_Called : BaseTest<TlevelController>
    {
        protected ITlevelLoader TlevelLoader;
        protected TlevelController Controller;
        protected Task<IActionResult> Result;
        
        protected long ukprn;
        protected int pathwayId;

        protected TlevelQueryViewModel expectedResult;

        public override void Setup()
        {
            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext.Returns(new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(CustomClaimTypes.Ukprn, ukprn.ToString())
                }))
            });

            TlevelLoader = Substitute.For<ITlevelLoader>();
            Controller = new TlevelController(TlevelLoader)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContextAccessor.HttpContext
                }
            };

            expectedResult = new TlevelQueryViewModel
            {
                PathwayId = 1,
                PathwayName = "Test Pathway",
                PathwayStatusId = 1,
                Query = "Test query",
                Specialisms = new List<string> { "Spl1", "Spl2" },
                TqAwardingOrganisationId = pathwayId
            };
        }

        public override void When()
        {
            Result = Controller.ReportIssueAsync(pathwayId);
        }
    }
}
