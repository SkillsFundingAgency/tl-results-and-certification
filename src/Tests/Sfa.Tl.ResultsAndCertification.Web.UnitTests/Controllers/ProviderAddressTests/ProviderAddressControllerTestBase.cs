using Microsoft.AspNetCore.Http;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests
{
    public abstract class ProviderAddressControllerTestBase : BaseTest<ProviderAddressController>
    {
        // Dependencies
        protected ProviderAddressController Controller;

        // HttpContext
        protected int ProviderUkprn;
        protected Guid UserId;
        protected IHttpContextAccessor HttpContextAccessor;
        protected string CacheKey;

        public override void Setup()
        {
            Controller = new ProviderAddressController();
            ProviderUkprn = 1234567890;
        }
    }
}
