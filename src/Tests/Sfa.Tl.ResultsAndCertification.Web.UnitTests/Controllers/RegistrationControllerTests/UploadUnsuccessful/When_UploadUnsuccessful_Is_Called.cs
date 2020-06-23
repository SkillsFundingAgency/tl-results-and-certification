using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.UploadUnsuccessful
{
    public abstract class When_UploadUnsuccessful_Is_Called : BaseTest<RegistrationController>
    {
        protected long Ukprn;
        protected IRegistrationLoader RegistrationLoader;
        protected ILogger<RegistrationController> Logger;
        protected RegistrationController Controller;
        protected UploadUnsuccessfulViewModel UploadUnsuccessfulViewModel;
        protected IHttpContextAccessor HttpContextAccessor;
        protected TempDataDictionary TempData;
        public IActionResult Result { get; private set; }
        protected Guid BlobUniqueReference;

        public override void Setup()
        {
            Ukprn = 12345;            
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            Logger = Substitute.For<ILogger<RegistrationController>>();
            RegistrationLoader = Substitute.For<IRegistrationLoader>();
            Controller = new RegistrationController(RegistrationLoader, Logger);

            var httpContext = new ClaimsIdentityBuilder<RegistrationController>(Controller)
               .Add(CustomClaimTypes.Ukprn, Ukprn.ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
            TempData = new TempDataDictionary(HttpContextAccessor.HttpContext, Substitute.For<ITempDataProvider>());
            Controller.TempData = TempData;
        }

        public override void When()
        {
            Result = Controller.UploadUnsuccessful();
        }
    }
}
