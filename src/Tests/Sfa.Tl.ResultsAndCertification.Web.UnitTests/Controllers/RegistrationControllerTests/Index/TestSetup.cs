using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.Index
{
    public abstract class TestSetup : BaseTest<RegistrationController>
    {
        protected IRegistrationLoader RegistrationLoader;
        protected ICacheService CacheService;
        protected ILogger<RegistrationController> Logger;
        protected RegistrationController Controller;
        public IActionResult Result { get; private set; }

        public override void Setup()
        {
            RegistrationLoader = Substitute.For<IRegistrationLoader>();
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<RegistrationController>>();
            Controller = new RegistrationController(RegistrationLoader, CacheService, Logger);
        }

        public override Task When()
        {
            //Result = Controller.Index();            
            var indexTask = Task.Run(() => Controller.Index());
            Result = indexTask.GetAwaiter().GetResult();
            return indexTask;
        }
    }
}
