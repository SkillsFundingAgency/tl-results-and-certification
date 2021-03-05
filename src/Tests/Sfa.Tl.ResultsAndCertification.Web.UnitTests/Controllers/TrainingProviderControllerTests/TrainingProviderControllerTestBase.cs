using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests
{
    public abstract class TrainingProviderControllerTestBase : BaseTest<TrainingProviderController>
    {
        // Dependencies
        protected ITrainingProviderLoader TrainingProviderLoader;
        protected ICacheService CacheService;
        protected ILogger<TrainingProviderController> Logger;
        protected TrainingProviderController Controller;

        // HttpContext
        protected int Ukprn;
        protected Guid UserId;
        protected IHttpContextAccessor HttpContextAccessor;

        public override void Setup()
        {
            TrainingProviderLoader = Substitute.For<ITrainingProviderLoader>();
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<TrainingProviderController>>();
            Controller = new TrainingProviderController(TrainingProviderLoader, CacheService, Logger);

            Ukprn = 1234567890;
            var httpContext = new ClaimsIdentityBuilder<TrainingProviderController>(Controller)
               .Add(CustomClaimTypes.Ukprn, Ukprn.ToString())
               .Add(CustomClaimTypes.UserId, Guid.NewGuid().ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            HttpContextAccessor.HttpContext.Returns(httpContext);
        }
    }
}
