using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.UploadRegistrationsFileGet
{
    public abstract class TestSetup : BaseTest<RegistrationController>
    {
        protected IRegistrationLoader RegistrationLoader;
        protected ICacheService CacheService;
        protected ILogger<RegistrationController> Logger;
        protected RegistrationController Controller;
        protected UploadRegistrationsRequestViewModel ViewModel;
        public IActionResult Result { get; private set; }

        public override void Setup()
        {
            RegistrationLoader = Substitute.For<IRegistrationLoader>();
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<RegistrationController>>();
            Controller = new RegistrationController(RegistrationLoader, CacheService, Logger);
            ViewModel = new UploadRegistrationsRequestViewModel();
        }

        public override Task When()
        {
            //Result = Controller.UploadRegistrationsFile();
            var uploadRegistrationsTask = Task.Run(() => Controller.UploadRegistrationsFile());
            Result = uploadRegistrationsTask.GetAwaiter().GetResult();
            return uploadRegistrationsTask;
        }
    }
}
