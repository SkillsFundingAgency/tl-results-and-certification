using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Filters.FreezePeriod
{
    public class Then_No_Access_Granted_To_Service : When_FilterAttribute_Action_Is_Called
    {
        private HomeController _homeController;
        private ILogger<HomeController> _logger;
        private ILogger<ErrorController> _errorLogger;
        private ErrorController _errorController;

        public override void Given()
        {
            ResultsAndCertificationConfiguration = new ResultsAndCertificationConfiguration { FreezePeriodStartDate = DateTime.UtcNow, FreezePeriodEndDate = DateTime.UtcNow.AddDays(5) };
            _logger = Substitute.For<ILogger<HomeController>>();
            _homeController = new HomeController(_logger);

            _errorLogger = Substitute.For<ILogger<ErrorController>>();
            _errorController = new ErrorController(ResultsAndCertificationConfiguration, _errorLogger);

            var httpContext = new ClaimsIdentityBuilder<HomeController>(_homeController)
               .Add(CustomClaimTypes.UserId, Guid.NewGuid().ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);

            var routeData = new RouteData();
            routeData.Values.Add("controller", "Home");
            routeData.Values.Add("action", nameof(HomeController.Index));

            var controllerActionDescriptor = new ControllerActionDescriptor
            {
                ControllerName = "Home",
                ActionName = nameof(HomeController.Index)
            };

            var actionContext = new ActionContext(HttpContextAccessor.HttpContext, routeData, controllerActionDescriptor, Substitute.For<ModelStateDictionary>());
            ActionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new Dictionary<string, object>(), _homeController);

            //mockDelegate = () => {
            //    return Task.FromResult(new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), _errorController));
            //};
        }

        [Fact(Skip = "TODO")]
        public void Then_Redirected_To_ServiceUnavailable()
        {
            var terst = ActionExecutingContext;
        }
    }
}
