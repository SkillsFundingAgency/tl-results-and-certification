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
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementImportControllerTests.UploadSuccessful
{
    public abstract class TestSetup : BaseTest<IndustryPlacementImportController>
    {
        protected long Ukprn;
        protected string CacheKey;
        protected IIndustryPlacementLoader IndustryPlacementLoader;
        protected ICacheService CacheService;
        protected ILogger<IndustryPlacementImportController> Logger;
        protected IndustryPlacementImportController Controller;
        protected UploadSuccessfulViewModel UploadSuccessfulViewModel;
        protected IHttpContextAccessor HttpContextAccessor;
        public IActionResult Result { get; private set; }
        protected Guid BlobUniqueReference;

        public override void Setup()
        {
            Ukprn = 12345;
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            Logger = Substitute.For<ILogger<IndustryPlacementImportController>>();
            IndustryPlacementLoader = Substitute.For<IIndustryPlacementLoader>();
            CacheService = Substitute.For<ICacheService>();
            Controller = new IndustryPlacementImportController(IndustryPlacementLoader, CacheService, Logger);

            var httpContext = new ClaimsIdentityBuilder<IndustryPlacementImportController>(Controller)
               .Add(CustomClaimTypes.Ukprn, Ukprn.ToString())
               .Add(CustomClaimTypes.UserId, Guid.NewGuid().ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
            CacheKey = CacheKeyHelper.GetCacheKey(httpContext.User.GetUserId(), CacheConstants.IpCacheKey);
        }

        public async override Task When()
        {
            Result = await Controller.UploadSuccessful();
        }
    }
}
