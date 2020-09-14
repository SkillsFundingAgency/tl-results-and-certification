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
using System.Threading.Tasks;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeLearnersNameGet
{
    public abstract class When_ChangeLearnersNameAsync_Is_Called : BaseTest<ManageRegistrationController>
    {
        protected int AoUkprn;
        protected int ProfileId;
        protected Guid UserId;
        protected string CacheKey;
        protected IRegistrationLoader RegistrationLoader;
        protected ICacheService CacheService;
        protected ILogger<ManageRegistrationController> Logger;
        protected ManageRegistrationController Controller;
        protected RegistrationViewModel ViewModel;
        protected IHttpContextAccessor HttpContextAccessor;
        public IActionResult Result { get; private set; }

        public override void Setup()
        {
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            RegistrationLoader = Substitute.For<IRegistrationLoader>();
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<ManageRegistrationController>>();
            Controller = new ManageRegistrationController(RegistrationLoader, CacheService, Logger);

            ProfileId = 1;
            AoUkprn = 1234567890;
            var httpContext = new ClaimsIdentityBuilder<ManageRegistrationController>(Controller)
               .Add(CustomClaimTypes.Ukprn, AoUkprn.ToString())
               .Add(CustomClaimTypes.UserId, Guid.NewGuid().ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
            CacheKey = CacheKeyHelper.GetCacheKey(httpContext.User.GetUserId(), CacheConstants.RegistrationCacheKey);
        }

        public async override Task When()
        {
            Result = await Controller.ChangeLearnersNameAsync(ProfileId);
        }
    }
}
