using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.Details
{
    public class Then_TlevelLoader_Returns_Expected_Result : When_Details_Action_Called
    {
        private TLevelDetailsViewModel mockresult;

        public override void Given()
        {
            mockresult = new TLevelDetailsViewModel 
            { 
                PageTitle = "Tlevel Details", 
                PathwayName = "Education", 
                ShowSomethingIsNotRight = true,
                RouteName = "Digital Education", 
                Specialisms = new List<string> { "Specialism1", "Specialism2" }
            };

            TlevelLoader.GetTlevelDetailsByPathwayIdAsync(ukPrn, id)
                .Returns(mockresult);
        }

        [Fact]
        public void Then_GetTlevelDetailsByPathwayIdAsync_Is_Called()
        {
            TlevelLoader.Received().GetTlevelDetailsByPathwayIdAsync(ukPrn, id);
        }

        [Fact]
        public void Then_Expected_Result_Is_Returned()
        {
            var viewResult = Result.Result as ViewResult;
            var expectedModel = viewResult.Model as TLevelDetailsViewModel;

            expectedModel.PageTitle.Should().Be(mockresult.PageTitle);
            expectedModel.PathwayName.Should().Be(mockresult.PathwayName);
            expectedModel.ShowSomethingIsNotRight.Should().Be(mockresult.ShowSomethingIsNotRight);
            expectedModel.RouteName.Should().Be(mockresult.RouteName);
            expectedModel.Specialisms.Count().Should().Be(2);
            expectedModel.Specialisms.First().Should().Be(mockresult.Specialisms.First());
        }
    }
}
