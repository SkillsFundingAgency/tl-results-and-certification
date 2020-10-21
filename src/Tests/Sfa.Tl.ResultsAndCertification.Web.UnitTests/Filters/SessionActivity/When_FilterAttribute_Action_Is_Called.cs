using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Filters;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Filters.SessionActivity
{
    public abstract class When_FilterAttribute_Action_Is_Called : BaseTest<SessionActivityFilterAttribute>
    {
        protected string CacheKey;        
        protected ICacheService CacheService;
        protected IHttpContextAccessor HttpContextAccessor;        
        protected ILogger<SessionActivityFilterAttribute> Logger;        
        protected ActionExecutingContext ActionExecutingContext;

        public override void Setup()
        {
            Logger = Substitute.For<ILogger<SessionActivityFilterAttribute>>();            
            CacheService = Substitute.For<ICacheService>();
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();            
        }

        public override Task When()
        {
            var filterAttribute = new SessionActivityFilterAttribute(CacheService, Logger);
            var actionExecutionDelegate = Substitute.For<ActionExecutionDelegate>();
            filterAttribute.OnActionExecutionAsync(ActionExecutingContext, actionExecutionDelegate).GetAwaiter().GetResult();
            return Task.CompletedTask;
        }
    }
}
