﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.AssessmentsDownloadDataLink
{
    public abstract class TestSetup : BaseTest<AssessmentController>
    {
        protected long Ukprn;
        protected IAssessmentLoader AssessmentLoader;
        protected ICacheService CacheService;
        protected ILogger<AssessmentController> Logger;
        protected AssessmentController Controller;
        protected IHttpContextAccessor HttpContextAccessor;
        public IActionResult Result { get; private set; }
        protected string Id;
        protected ComponentType ComponentType;

        public override void Setup()
        {
            Ukprn = 123456789;
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            Logger = Substitute.For<ILogger<AssessmentController>>();
            AssessmentLoader = Substitute.For<IAssessmentLoader>();
            CacheService = Substitute.For<ICacheService>();

            Controller = new AssessmentController(AssessmentLoader, CacheService, Logger);

            var httpContext = new ClaimsIdentityBuilder<AssessmentController>(Controller)
               .Add(CustomClaimTypes.Ukprn, Ukprn.ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);            
        }

        public async override Task When()
        {
            Result = await Controller.AssessmentsDownloadDataLinkAsync(Id, ComponentType);
        }
    }
}
