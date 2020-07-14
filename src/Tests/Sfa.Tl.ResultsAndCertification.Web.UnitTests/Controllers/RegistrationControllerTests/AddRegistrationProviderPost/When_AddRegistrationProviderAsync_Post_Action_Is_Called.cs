using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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


namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationProviderPost
{
    public abstract class When_AddRegistrationProviderAsync_Post_Action_Is_Called : BaseTest<RegistrationController>
    {
        protected int Ukprn;
        protected Guid UserId;
        protected string CacheKey;
        protected IRegistrationLoader RegistrationLoader;
        protected ICacheService CacheService;
        protected ILogger<RegistrationController> Logger;
        protected RegistrationController Controller;
        protected RegistrationViewModel ViewModel;
        protected DateofBirthViewModel DateofBirthViewModel;
        protected SelectProviderViewModel SelectProviderViewModel;
        protected IHttpContextAccessor HttpContextAccessor;
        public IActionResult Result { get; private set; }

        public override void Setup()
        {
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            RegistrationLoader = Substitute.For<IRegistrationLoader>();
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<RegistrationController>>();
            Controller = new RegistrationController(RegistrationLoader, CacheService, Logger);

            Ukprn = 1234567890;
            var httpContext = new ClaimsIdentityBuilder<RegistrationController>(Controller)
               .Add(CustomClaimTypes.Ukprn, Ukprn.ToString())
               .Add(CustomClaimTypes.UserId, Guid.NewGuid().ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
            CacheKey = CacheKeyHelper.GetCacheKey(httpContext.User.GetUserId(), CacheConstants.RegistrationCacheKey);
        }

        public override void When()
        {
            Result = Controller.AddRegistrationProviderAsync(SelectProviderViewModel).Result;
        }
    }
}
