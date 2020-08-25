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
namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DocumentControllerTests.DownloadRegistrationDataFormatAndRulesGuideGet
{
    public abstract class When_DownloadRegistrationDataFormatAndRulesGuide_Action_Is_Called : BaseTest<DocumentController>
    {
        protected Guid UserId;
        protected IDocumentLoader DocumentLoader;
        protected ICacheService CacheService;
        protected ILogger<DocumentController> Logger;
        protected DocumentController Controller;
        protected IHttpContextAccessor HttpContextAccessor;
        public IActionResult Result { get; private set; }

        public override void Setup()
        {
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            DocumentLoader = Substitute.For<IDocumentLoader>();
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<DocumentController>>();
            Controller = new DocumentController(DocumentLoader, Logger);

            var httpContext = new ClaimsIdentityBuilder<DocumentController>(Controller)
               .Add(CustomClaimTypes.UserId, Guid.NewGuid().ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
        }

        public override void When()
        {
            Result = Controller.DownloadRegistrationDataFormatAndRulesGuideAsync().Result;
        }
    }
}
