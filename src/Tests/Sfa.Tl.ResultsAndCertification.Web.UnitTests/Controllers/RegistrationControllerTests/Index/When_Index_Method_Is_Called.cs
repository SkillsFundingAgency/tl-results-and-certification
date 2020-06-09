using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.Index
{
    public abstract class When_Index_Method_Is_Called : BaseTest<RegistrationController>
    {
        protected IRegistrationLoader RegistrationLoader;
        protected ILogger<RegistrationController> Logger;
        protected RegistrationController Controller;
        public IActionResult Result { get; private set; }

        public override void Setup()
        {
            RegistrationLoader = Substitute.For<IRegistrationLoader>();
            Logger = Substitute.For<ILogger<RegistrationController>>();
            Controller = new RegistrationController(RegistrationLoader, Logger);
        }

        public override void When()
        {
            Result = Controller.Index();
        }
    }
}
