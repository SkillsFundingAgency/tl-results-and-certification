using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.Index
{
    public class When_Index_Action_Called : BaseTest<TlevelController>
    {
        protected IAwardingOrganisationLoader AwardingOrganistionLoader;
        protected TlevelController Controller;
        protected Task<IActionResult> Result;

        public override void Setup()
        {
            AwardingOrganistionLoader = Substitute.For<IAwardingOrganisationLoader>();
            Controller = new TlevelController(AwardingOrganistionLoader);
        }

        public override void Given()
        {
            var mockresult = new List<YourTlevelsViewModel>
            {
                    new YourTlevelsViewModel { PathId = 1, RouteId = 1, TLevelStatus = "Confirmed", TLevelDescription = "RouteName1: Pathway1" },
                    new YourTlevelsViewModel { PathId = 2, RouteId = 2, TLevelStatus = "Confirmed", TLevelDescription = "RouteName2: Pathway2"}
            };
            AwardingOrganistionLoader.GetTlevelsByAwardingOrganisationAsync()
                .Returns(mockresult);
        }

        public override void When()
        {
            Result = Controller.Index();
        }
    }
}
