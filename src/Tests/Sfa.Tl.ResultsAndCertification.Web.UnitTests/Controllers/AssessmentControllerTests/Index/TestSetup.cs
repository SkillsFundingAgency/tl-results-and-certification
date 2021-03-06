﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.Index
{
    public abstract class TestSetup : BaseTest<AssessmentController>
    {
        protected IAssessmentLoader AssessmentLoader;
        protected ICacheService CacheService;
        protected ILogger<AssessmentController> Logger;
        protected AssessmentController Controller;
        public IActionResult Result { get; private set; }

        public override void Setup()
        {
            AssessmentLoader = Substitute.For<IAssessmentLoader>();
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<AssessmentController>>();
            Controller = new AssessmentController(AssessmentLoader, CacheService, Logger);
        }

        public override Task When()
        {
            Result = Controller.Index();
            return Task.CompletedTask;
        }
    }
}
