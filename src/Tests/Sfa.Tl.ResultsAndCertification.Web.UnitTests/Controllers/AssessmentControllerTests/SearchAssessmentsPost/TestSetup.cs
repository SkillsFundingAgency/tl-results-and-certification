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
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.SearchAssessmentsPost
{
    public abstract class TestSetup : BaseTest<AssessmentController>
    {
        protected int AoUkprn;
        protected string SearchUln;
        protected Guid UserId;
        protected string CacheKey;
        protected IAssessmentLoader AssessmentLoader;
        protected ICacheService CacheService;
        protected ILogger<AssessmentController> Logger;
        protected AssessmentController Controller;
        protected SearchAssessmentsViewModel SearchAssessmentsViewModel;

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
            SearchUln = "1259874562";
            var httpContext = new ClaimsIdentityBuilder<AssessmentController>(Controller)
               .Add(CustomClaimTypes.Ukprn, AoUkprn.ToString())
               .Add(CustomClaimTypes.UserId, Guid.NewGuid().ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
        }

        public async override Task When()
        {
            Result = await Controller.SearchAssessmentsAsync(SearchAssessmentsViewModel);
        }
    }
}
