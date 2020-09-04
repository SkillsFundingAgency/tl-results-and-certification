using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Filters.SessionActivity
{
    public class Then_SessionActivity_Is_Not_Recorded : When_SessionActivityFilterAttribute_Action_Is_Called
    {
        private TimeoutController _timeoutController;
        private ResultsAndCertificationConfiguration _resultsAndCertificationConfiguration;
        protected ActionExecutingContext _actionExecutingContext;

        public override void Given()
        {
            _resultsAndCertificationConfiguration = new ResultsAndCertificationConfiguration { DfeSignInSettings = new DfeSignInSettings { Timeout = 2 } };
            _timeoutController = new TimeoutController(_resultsAndCertificationConfiguration, CacheService);

            var httpContext = new ClaimsIdentityBuilder<TimeoutController>(_timeoutController)
               .Add(CustomClaimTypes.UserId, Guid.NewGuid().ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
            CacheKey = CacheKeyHelper.GetCacheKey(HttpContextAccessor.HttpContext.User.GetUserId(), CacheConstants.UserSessionActivityCacheKey);

            var routeData = new RouteData();
            routeData.Values.Add("controller", "Timeout");
            routeData.Values.Add("action", nameof(TimeoutController.GetActiveDurationAsync));

            var controllerActionDescriptor = new ControllerActionDescriptor
            {
                ControllerName = "Timeout",
                ActionName = nameof(TimeoutController.GetActiveDurationAsync)
            };

            var actionContext = new ActionContext(HttpContextAccessor.HttpContext, routeData, controllerActionDescriptor);
            _actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new Dictionary<string, object>(), _timeoutController);
        }

        [Fact]
        public void Then_SessionActivity_Cache_Is_Not_Synchronised()
        {
            CacheService.DidNotReceive().SetAsync(CacheKey, Arg.Any<DateTime>());
        }
    }
}
