﻿using Microsoft.AspNetCore.Http;
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
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.SubmitAssessmentsGeneratingDownload
{
    public abstract class TestSetup : BaseTest<RegistrationController>
    {
        protected int AoUkprn;
        protected Guid UserId;
        protected string CacheKey;
        protected IAssessmentLoader AssessmentLoader;
        protected ICacheService CacheService;
        protected ILogger<AssessmentController> Logger;
        protected AssessmentController Controller;        
        protected IHttpContextAccessor HttpContextAccessor;
        public IActionResult Result { get; private set; }

        public override void Setup()
        {
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            AssessmentLoader = Substitute.For<IAssessmentLoader>();
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<AssessmentController>>();
            Controller = new AssessmentController(AssessmentLoader, CacheService, Logger);
            
            AoUkprn = 1234567890;
            var httpContext = new ClaimsIdentityBuilder<AssessmentController>(Controller)
               .Add(CustomClaimTypes.Ukprn, AoUkprn.ToString())
               .Add(CustomClaimTypes.UserId, Guid.NewGuid().ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
            CacheKey = CacheKeyHelper.GetCacheKey(httpContext.User.GetUserId(), CacheConstants.AssessmentCacheKey);

        }

        public async override Task When()
        {
            Result = await Controller.SubmitAssessmentsGeneratingDownloadAsync();
        }
    }
}
