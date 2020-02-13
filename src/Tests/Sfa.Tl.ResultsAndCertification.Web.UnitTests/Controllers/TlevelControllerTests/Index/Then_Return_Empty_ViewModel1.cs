using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.Models;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.Index
{
    public class Then_Return_Empty_ViewModel1 : When_Index_Action_Called
    {
        public override void Given()
        {
            var mockresult = new List<YourTlevelsViewModel>();
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
            Assert.True(model.Count == 0);
        }
    }
}
