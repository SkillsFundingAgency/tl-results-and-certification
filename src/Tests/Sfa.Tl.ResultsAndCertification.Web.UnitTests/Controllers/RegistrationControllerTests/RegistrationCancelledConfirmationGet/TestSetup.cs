using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.RegistrationCancelledConfirmationGet
{
    public abstract class TestSetup : BaseTest<RegistrationController>
    {
        protected long AoUkprn;
        protected Guid UserId;
        protected string CacheKey;
        protected ICacheService CacheService;
        protected IRegistrationLoader RegistrationLoader;
        protected ILogger<RegistrationController> Logger;
        protected RegistrationController Controller;
        protected IHttpContextAccessor HttpContextAccessor;
        protected TempDataDictionary TempData;
        protected RegistrationCancelledConfirmationViewModel RegistrationCancelledConfirmationViewModel;
        public IActionResult Result { get; private set; }

        public override void Setup()
        {
            AoUkprn = 1234567890;
            UserId = Guid.NewGuid();
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            RegistrationLoader = Substitute.For<IRegistrationLoader>();
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<RegistrationController>>();
            Controller = new RegistrationController(RegistrationLoader, CacheService, Logger);

            var httpContext = new ClaimsIdentityBuilder<RegistrationController>(Controller)
               .Add(CustomClaimTypes.Ukprn, AoUkprn.ToString())
               .Add(CustomClaimTypes.UserId, UserId.ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
            CacheKey = CacheKeyHelper.GetCacheKey(httpContext.User.GetUserId(), CacheConstants.RegistrationCacheKey);
            //TempData = new TempDataDictionary(HttpContextAccessor.HttpContext, Substitute.For<ITempDataProvider>());
            //Controller.TempData = TempData;
        }

        public override void When()
        {
            Result = Controller.RegistrationCancelledConfirmationAsync().Result;
        }
    }
}
