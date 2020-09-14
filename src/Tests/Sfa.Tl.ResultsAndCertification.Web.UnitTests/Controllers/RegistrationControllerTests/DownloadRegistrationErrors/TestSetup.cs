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

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.DownloadRegistrationErrors
{
    public abstract class TestSetup : BaseTest<RegistrationController>
    {
        protected long Ukprn;
        protected IRegistrationLoader RegistrationLoader;
        protected ICacheService CacheService;
        protected ILogger<RegistrationController> Logger;
        protected RegistrationController Controller;
        protected IHttpContextAccessor HttpContextAccessor;
        public Task<IActionResult> Result { get; private set; }
        protected Guid BlobUniqueReference;
        protected string Id;

        public override void Setup()
        {
            Ukprn = 123456789;
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            Logger = Substitute.For<ILogger<RegistrationController>>();
            RegistrationLoader = Substitute.For<IRegistrationLoader>();
            CacheService = Substitute.For<ICacheService>();

            Controller = new RegistrationController(RegistrationLoader, CacheService, Logger);

            var httpContext = new ClaimsIdentityBuilder<RegistrationController>(Controller)
               .Add(CustomClaimTypes.Ukprn, Ukprn.ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
            RegistrationLoader.GetRegistrationValidationErrorsFileAsync(Ukprn, BlobUniqueReference).Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test File for validation errors")));
        }

        public override void When()
        {
            Result = Controller.DownloadRegistrationErrors(Id);
        }
    }
}
