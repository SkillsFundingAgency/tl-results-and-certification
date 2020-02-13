using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.Index
{
    public class Then_Return_TwoRecord_ViewModel : When_Index_Action_Called
    {
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

        [Fact]
        public void Then_GetTlevelsByAwardingOrganisationAsync_Is_Called()
        {
            AwardingOrganistionLoader.Received().GetTlevelsByAwardingOrganisationAsync();
        }

        [Fact]
        public void Then_GetTlevelsByAwardingOrganisationAsync_ViewModel_Return_Two_Rows()
        {
            var viewResult = Result.Result as ViewResult;
            var model = viewResult.Model as IList<YourTlevelsViewModel>;
            Assert.True(model.Count == 2);
        }

        [Fact]
        public void Then_GetTlevelsByAwardingOrganisationAsync_Index_Returns_Expected_ViewModel()
        {
            var viewResult = Result.Result as ViewResult;
            var model = viewResult.Model as IList<YourTlevelsViewModel>;

            var expectedModel = model.FirstOrDefault();
            Assert.True(expectedModel.PathId == 1);
            Assert.True(expectedModel.RouteId == 1);
            Assert.True(expectedModel.TLevelStatus.Equals("Confirmed"));
            Assert.True(expectedModel.TLevelDescription.Equals("RouteName1: Pathway1"));
        }
    }
}
