using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Filters.SessionActivity
{
    public class Then_Home_Activity_Not_Recorded : When_FilterAttribute_Action_Is_Called
    {
        private HomeController _homeController;
        private ILogger<HomeController> _logger;
        protected ActionExecutingContext _actionExecutingContext;

        public override void Given()
        {
            _homeController = new HomeController(_logger);

            var httpContext = new ClaimsIdentityBuilder<HomeController>(_homeController)
               .Add(CustomClaimTypes.UserId, Guid.NewGuid().ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
            CacheKey = CacheKeyHelper.GetCacheKey(HttpContextAccessor.HttpContext.User.GetUserId(), CacheConstants.UserSessionActivityCacheKey);

            var routeData = new RouteData();
            routeData.Values.Add("controller", "Home");
            routeData.Values.Add("action", nameof(HomeController.Index));

            var controllerActionDescriptor = new ControllerActionDescriptor
            {
                ControllerName = "Home",
                ActionName = nameof(HomeController.Index)
            };

            var actionContext = new ActionContext(HttpContextAccessor.HttpContext, routeData, controllerActionDescriptor);
            _actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new Dictionary<string, object>(), _homeController);
        }

        [Fact]
        public void Then_SessionActivity_Is_Not_Recored()
        {
            CacheService.DidNotReceive().SetAsync(CacheKey, Arg.Any<DateTime>());
        }
    }
}
