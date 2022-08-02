using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Filters;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Filters.FreezePeriod
{
    public abstract class When_FilterAttribute_Action_Is_Called : BaseTest<FreezePeriodFilterAttribute>
    {
        protected IHttpContextAccessor HttpContextAccessor;
        protected ILogger<FreezePeriodFilterAttribute> Logger;
        protected ActionExecutingContext ActionExecutingContext;
        protected ResultsAndCertificationConfiguration ResultsAndCertificationConfiguration;
        protected ActionExecutionDelegate mockDelegate;

        public override void Setup()
        {
            Logger = Substitute.For<ILogger<FreezePeriodFilterAttribute>>();
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
        }

        public override Task When()
        {
            var filterAttribute = new FreezePeriodFilterAttribute(ResultsAndCertificationConfiguration, Logger);
            var actionExecutionDelegate = Substitute.For<ActionExecutionDelegate>();
            filterAttribute.OnActionExecutionAsync(ActionExecutingContext, mockDelegate).GetAwaiter().GetResult();
            return Task.CompletedTask;
        }
    }
}
