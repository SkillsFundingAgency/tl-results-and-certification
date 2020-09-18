using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
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
    public class Then_Signout_Activity_Not_Recorded : When_FilterAttribute_Action_Is_Called
    {
        private DashboardController _dashboardController;
        private ILogger<DashboardController> _logger;
        public override void Given()
        {
            _logger = Substitute.For<ILogger<DashboardController>>();
            _dashboardController = new DashboardController(_logger);

            var httpContext = new ClaimsIdentityBuilder<DashboardController>(_dashboardController)
               .Add(CustomClaimTypes.HasAccessToService, "true")
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
            CacheKey = CacheKeyHelper.GetCacheKey(HttpContextAccessor.HttpContext.User.GetUserId(), CacheConstants.UserSessionActivityCacheKey);

            var routeData = new RouteData();
            routeData.Values.Add("controller", "Dashboard");
            routeData.Values.Add("action", nameof(DashboardController.Index));

            var controllerActionDescriptor = new ControllerActionDescriptor
            {
                ControllerName = "Dashboard",
                ActionName = nameof(DashboardController.Index)
            };

            var actionContext = new ActionContext(HttpContextAccessor.HttpContext, routeData, controllerActionDescriptor);
            ActionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new Dictionary<string, object>(), _dashboardController);
        }

        [Fact]
        public void Then_SessionActivity_Is_Not_Recored()
        {
            CacheService.DidNotReceive().SetAsync(CacheKey, Arg.Any<DateTime>());
        }
    }
}
