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
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.DownloadResultErrors
{
    public abstract class TestSetup : BaseTest<ResultController>
    {
        protected long Ukprn;
        protected IResultLoader ResultLoader;
        protected ICacheService CacheService;
        protected ILogger<ResultController> Logger;
        protected ResultController Controller;
        protected IHttpContextAccessor HttpContextAccessor;
        public IActionResult Result { get; private set; }
        protected Guid BlobUniqueReference;
        protected string Id;

        public override void Setup()
        {
            Ukprn = 123456789;
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            Logger = Substitute.For<ILogger<ResultController>>();
            ResultLoader = Substitute.For<IResultLoader>();
            CacheService = Substitute.For<ICacheService>();

            Controller = new ResultController(ResultLoader, CacheService, Logger);

            var httpContext = new ClaimsIdentityBuilder<ResultController>(Controller)
               .Add(CustomClaimTypes.Ukprn, Ukprn.ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
            ResultLoader.GetResultValidationErrorsFileAsync(Ukprn, BlobUniqueReference).Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test File for validation errors")));
        }

        public async override Task When()
        {
            Result = await Controller.DownloadResultErrors(Id);
        }
    }
}
