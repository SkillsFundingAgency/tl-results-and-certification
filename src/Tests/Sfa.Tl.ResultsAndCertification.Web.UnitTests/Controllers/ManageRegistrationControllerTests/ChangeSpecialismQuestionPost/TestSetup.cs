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
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeSpecialismQuestionPost
{
    public abstract class TestSetup : BaseTest<ManageRegistrationController>
    {
        protected int AoUkprn;
        protected int ProfileId;
        protected Guid UserId;
        protected bool IsChangeMode;
        protected IRegistrationLoader RegistrationLoader;
        protected ICacheService CacheService;
        protected ILogger<ManageRegistrationController> Logger;
        protected ManageRegistrationController Controller;
        protected IHttpContextAccessor HttpContextAccessor;
        protected ChangeSpecialismQuestionViewModel ViewModel;
        public IActionResult Result { get; private set; }
        protected ManageRegistrationResponse MockResult;

        public override void Setup()
        {
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            RegistrationLoader = Substitute.For<IRegistrationLoader>();
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<ManageRegistrationController>>();
            Controller = new ManageRegistrationController(RegistrationLoader, CacheService, Logger);

            ProfileId = 1;
            AoUkprn = 1234567890;
            var httpContext = new ClaimsIdentityBuilder<ManageRegistrationController>(Controller)
               .Add(CustomClaimTypes.Ukprn, AoUkprn.ToString())
               .Add(CustomClaimTypes.UserId, Guid.NewGuid().ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
            ViewModel = new ChangeSpecialismQuestionViewModel();
            MockResult = new ManageRegistrationResponse();
        }

        public override void When()
        {
            Result = Controller.ChangeRegistrationSpecialismQuestionAsync(ViewModel).Result;
        }
    }
}
