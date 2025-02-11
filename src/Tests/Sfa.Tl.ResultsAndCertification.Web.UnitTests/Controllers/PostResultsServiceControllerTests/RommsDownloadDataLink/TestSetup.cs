using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.PostResultsService.RommsDownloadDataLink
{
    public abstract class TestSetup : BaseTest<PostResultsServiceController>
    {
        protected long Ukprn;
        protected IPostResultsServiceLoader PostResultsServiceLoader;
        protected ICacheService CacheService;
        protected ILogger<PostResultsServiceController> Logger;
        protected PostResultsServiceController Controller;
        protected IHttpContextAccessor HttpContextAccessor;
        public IActionResult Result { get; private set; }
        protected Guid BlobUniqueReference;
        protected string Id;

        public override void Setup()
        {
            Ukprn = 123456789;
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            Logger = Substitute.For<ILogger<PostResultsServiceController>>();
            PostResultsServiceLoader = Substitute.For<IPostResultsServiceLoader>();
            CacheService = Substitute.For<ICacheService>();

            Controller = new PostResultsServiceController(PostResultsServiceLoader, CacheService, Logger);

            var httpContext = new ClaimsIdentityBuilder<PostResultsServiceController>(Controller)
               .Add(CustomClaimTypes.Ukprn, Ukprn.ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
        }

        public async override Task When()
        {
            Result = await Controller.RommsDownloadDataLinkAsync(Id);
        }
    }
}
