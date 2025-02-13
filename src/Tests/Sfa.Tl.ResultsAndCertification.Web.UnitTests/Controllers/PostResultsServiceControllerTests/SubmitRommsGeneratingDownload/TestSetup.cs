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

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.SubmitRommsGeneratingDownload
{
    public abstract class TestSetup : BaseTest<PostResultsServiceController>
    {
        protected const int AoUkprn = 1234567890;
        protected const string UserEmail = "user@email.com";

        protected string CacheKey;
        protected IPostResultsServiceLoader PostResultsServiceLoader;
        protected ICacheService CacheService;
        private PostResultsServiceController _controller;

        protected IActionResult Result { get; private set; }

        public override void Setup()
        {
            PostResultsServiceLoader = Substitute.For<IPostResultsServiceLoader>();
            CacheService = Substitute.For<ICacheService>();

            _controller = new PostResultsServiceController(PostResultsServiceLoader, CacheService, Substitute.For<ILogger<PostResultsServiceController>>());

            var httpContext = new ClaimsIdentityBuilder<PostResultsServiceController>(_controller)
               .Add(CustomClaimTypes.Ukprn, AoUkprn.ToString())
               .Add(CustomClaimTypes.UserId, Guid.NewGuid().ToString())
               .Add(ClaimTypes.Email, UserEmail)
               .Build()
               .HttpContext;

            IHttpContextAccessor httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext.Returns(httpContext);

            CacheKey = CacheKeyHelper.GetCacheKey(httpContext.User.GetUserId(), CacheConstants.PrsCacheKey);
        }

        public async override Task When()
        {
            Result = await _controller.SubmitRommsGeneratingDownloadAsync();
        }
    }
}
