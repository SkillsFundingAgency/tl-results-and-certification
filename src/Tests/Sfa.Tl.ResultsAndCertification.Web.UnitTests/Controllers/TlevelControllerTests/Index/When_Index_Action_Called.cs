using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.Index
{
    public abstract class When_Index_Action_Called : BaseTest<TlevelController>
    {
        protected IAwardingOrganisationLoader AwardingOrganistionLoader;
        protected TlevelController Controller;
        protected Task<IActionResult> Result;

        public override void Setup()
        {
            AwardingOrganistionLoader = Substitute.For<IAwardingOrganisationLoader>();
            Controller = new TlevelController(AwardingOrganistionLoader);
        }

        public override void When()
        {
            Result = Controller.Index();
        }
    }
}
