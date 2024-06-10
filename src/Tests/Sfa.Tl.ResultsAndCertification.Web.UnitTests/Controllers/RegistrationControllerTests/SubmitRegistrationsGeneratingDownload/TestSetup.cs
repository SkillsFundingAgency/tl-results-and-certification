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
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.SubmitRegistrationsGeneratingDownload
{
    public abstract class TestSetup : BaseTest<RegistrationController>
    {
        protected const int AoUkprn = 1234567890;
        protected const string UserEmail = "user@email.com";

        protected string CacheKey;
        protected IRegistrationLoader RegistrationLoader;
        protected ICacheService CacheService;
        private RegistrationController _controller;

        protected IActionResult Result { get; private set; }

        public override void Setup()
        {
            RegistrationLoader = Substitute.For<IRegistrationLoader>();
            CacheService = Substitute.For<ICacheService>();

            _controller = new RegistrationController(RegistrationLoader, CacheService, Substitute.For<ILogger<RegistrationController>>());

            var httpContext = new ClaimsIdentityBuilder<RegistrationController>(_controller)
               .Add(CustomClaimTypes.Ukprn, AoUkprn.ToString())
               .Add(CustomClaimTypes.UserId, Guid.NewGuid().ToString())
               .Add(ClaimTypes.Email, UserEmail)
               .Build()
               .HttpContext;

            IHttpContextAccessor httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext.Returns(httpContext);

            CacheKey = CacheKeyHelper.GetCacheKey(httpContext.User.GetUserId(), CacheConstants.RegistrationCacheKey);
        }

        public async override Task When()
        {
            Result = await _controller.SubmitRegistrationsGeneratingDownloadAsync();
        }
    }
}
