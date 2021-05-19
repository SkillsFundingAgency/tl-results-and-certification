using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests
{
    public abstract class StatementOfAchievementControllerTestBase : BaseTest<StatementOfAchievementController>
    {
        // Dependencies
        protected IProviderAddressLoader ProviderAddressLoader;
        protected ICacheService CacheService;
        protected ILogger<StatementOfAchievementController> Logger;
        protected StatementOfAchievementController Controller;

        // HttpContext
        protected int ProviderUkprn;
        protected Guid UserId;
        protected IHttpContextAccessor HttpContextAccessor;
        protected ResultsAndCertificationConfiguration ResultsAndCertificationConfiguration;

        public override void Setup()
        {
            ResultsAndCertificationConfiguration = new ResultsAndCertificationConfiguration { SoaAvailableDate = "10/08/2021".ToDateTime() };
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<StatementOfAchievementController>>();
            ProviderAddressLoader = Substitute.For<IProviderAddressLoader>();
            Controller = new StatementOfAchievementController(ProviderAddressLoader, ResultsAndCertificationConfiguration, Logger);
            
            ProviderUkprn = 1234567890;
            var httpContext = new ClaimsIdentityBuilder<StatementOfAchievementController>(Controller)
               .Add(CustomClaimTypes.Ukprn, ProviderUkprn.ToString())
               .Add(CustomClaimTypes.UserId, Guid.NewGuid().ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            HttpContextAccessor.HttpContext.Returns(httpContext);
        }
    }
}