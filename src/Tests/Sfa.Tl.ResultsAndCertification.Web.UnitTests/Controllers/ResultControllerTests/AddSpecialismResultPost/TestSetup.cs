﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.AddSpecialismResultPost
{
    public abstract class TestSetup : BaseTest<ResultController>
    {
        protected int AoUkprn;
        protected int ProfileId;
        protected Guid UserId;
        protected string CacheKey;
        protected IResultLoader ResultLoader;
        protected ICacheService CacheService;
        protected ILogger<ResultController> Logger;
        protected ResultController Controller;
        protected ManageSpecialismResultViewModel ViewModel;
        protected IHttpContextAccessor HttpContextAccessor;
        protected AddResultResponse MockResult;

        public IActionResult Result { get; private set; }

        public override void Setup()
        {
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            ResultLoader = Substitute.For<IResultLoader>();
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<ResultController>>();
            Controller = new ResultController(ResultLoader, CacheService, Logger);

            ProfileId = 1;
            AoUkprn = 1234567890;

            ViewModel = new ManageSpecialismResultViewModel
            {
                ProfileId = ProfileId,
                AssessmentId = 1,
                SelectedGradeCode = "PCG1"
            };

            var httpContext = new ClaimsIdentityBuilder<ResultController>(Controller)
               .Add(CustomClaimTypes.Ukprn, AoUkprn.ToString())
               .Add(CustomClaimTypes.UserId, Guid.NewGuid().ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
            CacheKey = CacheKeyHelper.GetCacheKey(httpContext.User.GetUserId(), CacheConstants.ResultCacheKey);
            MockResult = new AddResultResponse();
        }

        public async override Task When()
        {
            Result = await Controller.AddSpecialismResultAsync(ViewModel);
        }
    }
}
