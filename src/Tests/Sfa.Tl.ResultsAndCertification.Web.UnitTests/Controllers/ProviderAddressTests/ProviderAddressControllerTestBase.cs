using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests
{
    public abstract class ProviderAddressControllerTestBase : BaseTest<ProviderAddressController>
    {
        // Dependencies
        protected IProviderAddressLoader ProviderAddressLoader;
        protected ICacheService CacheService;
        protected ILogger<ProviderAddressController> Logger;
        protected ProviderAddressController Controller;

        // HttpContext
        protected int ProviderUkprn;
        protected Guid UserId;
        protected IHttpContextAccessor HttpContextAccessor;
        protected string CacheKey;

        public override void Setup()
        {
            ProviderAddressLoader = Substitute.For<IProviderAddressLoader>();
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<ProviderAddressController>>();
            Controller = new ProviderAddressController(ProviderAddressLoader, CacheService, Logger);

            ProviderUkprn = 1234567890;
            var httpContext = new ClaimsIdentityBuilder<ProviderAddressController>(Controller)
               .Add(CustomClaimTypes.Ukprn, ProviderUkprn.ToString())
               .Add(CustomClaimTypes.UserId, Guid.NewGuid().ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            HttpContextAccessor.HttpContext.Returns(httpContext);

            CacheKey = CacheKeyHelper.GetCacheKey(httpContext.User.GetUserId(), CacheConstants.ProviderAddressCacheKey);
        }
    }
}
