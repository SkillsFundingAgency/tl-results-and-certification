using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.Index
{
    public class Then_Return_TwoRecord_ViewModel : When_Index_Action_Called
    {
        private List<YourTlevelsViewModel> mockresult;

        public override void Given()
        {
            mockresult = new List<YourTlevelsViewModel>
            {
                    new YourTlevelsViewModel { PathwayId = 1, StatusId = 1, TlevelTitle = "RouteName1: Pathway1" },
                    new YourTlevelsViewModel { PathwayId = 2, StatusId = 2, TlevelTitle = "RouteName2: Pathway2"}
            };
            TlevelLoader.GetAllTlevelsByUkprnAsync(Arg.Any<long>())
                .Returns(mockresult);
        }

        [Fact(Skip = "TODO: update required as per latest story")]
        public void Then_GetTlevelsByUkprnAsync_Is_Called()
        {
            TlevelLoader.Received().GetAllTlevelsByUkprnAsync(Arg.Any<long>());
        }

        [Fact(Skip="TODO: update required as per latest story")]
        public void Then_GetTlevelsByUkprnAsync_ViewModel_Return_Two_Rows()
        {
            var viewResult = Result.Result as ViewResult;
            var model = viewResult.Model as IList<YourTlevelsViewModel>;
            Assert.True(model.Count == 2);
        }

        [Fact(Skip = "TODO: update required as per latest story")]
        public void Then_GetTlevelsByUkprnAsync_Index_Returns_Expected_ViewModel()
        {
            var viewResult = Result.Result as ViewResult;
            var model = viewResult.Model as IList<YourTlevelsViewModel>;

            model.Should().NotBeNullOrEmpty();
            model.Count().Should().Be(2);
            
            var expectedFirstModel = model.FirstOrDefault();
            var actualModel = mockresult.FirstOrDefault();
            
            expectedFirstModel.PathwayId.Should().Be(actualModel.PathwayId);
            expectedFirstModel.StatusId.Should().Be(actualModel.StatusId);
            expectedFirstModel.TlevelTitle.Should().Be(actualModel.TlevelTitle);
        }
    }
}
