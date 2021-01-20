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
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.UploadSuccessful
{
    public abstract class TestSetup : BaseTest<ResultController>
    {
        protected long Ukprn;
        protected string CacheKey;
        protected IResultLoader ResultLoader;
        protected ICacheService CacheService;
        protected ILogger<ResultController> Logger;
        protected ResultController Controller;
        protected UploadSuccessfulViewModel UploadSuccessfulViewModel;
        protected IHttpContextAccessor HttpContextAccessor;
        public IActionResult Result { get; private set; }
        protected Guid BlobUniqueReference;

        public override void Setup()
        {
            Ukprn = 12345;
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            Logger = Substitute.For<ILogger<ResultController>>();
            ResultLoader = Substitute.For<IResultLoader>();
            CacheService = Substitute.For<ICacheService>();
            Controller = new ResultController(ResultLoader, CacheService, Logger);

            var httpContext = new ClaimsIdentityBuilder<ResultController>(Controller)
               .Add(CustomClaimTypes.Ukprn, Ukprn.ToString())
               .Add(CustomClaimTypes.UserId, Guid.NewGuid().ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
            CacheKey = string.Concat(CacheKeyHelper.GetCacheKey(httpContext.User.GetUserId(), CacheConstants.ResultCacheKey), Common.Helpers.Constants.ResultsUploadSuccessfulViewModel);
        }

        public async override Task When()
        {
            Result = await Controller.UploadSuccessful();
        }
    }
}
