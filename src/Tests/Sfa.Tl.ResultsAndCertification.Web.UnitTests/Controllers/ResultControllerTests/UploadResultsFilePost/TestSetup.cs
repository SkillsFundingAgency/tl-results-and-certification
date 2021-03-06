﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.UploadResultsFilePost
{
    public abstract class TestSetup : BaseTest<ResultController>
    {
        protected IResultLoader ResultLoader;
        protected ICacheService CacheService;
        protected ILogger<ResultController> Logger;

        protected long Ukprn;
        protected ResultController Controller;
        protected UploadResultsRequestViewModel ViewModel;
        protected UploadResultsResponseViewModel ResponseViewModel;
        protected IFormFile FormFile;
        protected IHttpContextAccessor HttpContextAccessor;
        public IActionResult Result { get; private set; }
        protected Guid BlobUniqueReference;

        public override void Setup()
        {
            Ukprn = 12345;
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            ResultLoader = Substitute.For<IResultLoader>();
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<ResultController>>();

            Controller = new ResultController(ResultLoader, CacheService, Logger);
            ViewModel = new UploadResultsRequestViewModel();

            var httpContext = new ClaimsIdentityBuilder<ResultController>(Controller)
               .Add(CustomClaimTypes.Ukprn, Ukprn.ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
        }

        public async override Task When()
        {
            Result = await Controller.UploadResultsFileAsync(ViewModel);
        }
    }
}
