using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TimeoutControllerTests.RenewSessionActivity
{
    public abstract class When_RenewSessionActivityAsync_Action_Is_Called : BaseTest<TimeoutController>
    {
        protected string CacheKey;
        protected TimeoutController Controller;
        protected JsonResult Result;
        protected ICacheService CacheService;
        protected IHttpContextAccessor HttpContextAccessor;
        protected ResultsAndCertificationConfiguration _resultsAndCertificationConfiguration;

        public override void Setup()
        {
            _resultsAndCertificationConfiguration = new ResultsAndCertificationConfiguration { DfeSignInSettings = new DfeSignInSettings { Timeout = 2 } };
            CacheService = Substitute.For<ICacheService>();
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            Controller = new TimeoutController(_resultsAndCertificationConfiguration, CacheService);

            var httpContext = new ClaimsIdentityBuilder<TimeoutController>(Controller)
               .Add(CustomClaimTypes.UserId, Guid.NewGuid().ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
            CacheKey = CacheKeyHelper.GetCacheKey(httpContext.User.GetUserId(), CacheConstants.UserSessionActivityCacheKey);
        }

        public async override Task When()
        {
            Result = await Controller.RenewSessionActivityAsync();
        }
    }
}
