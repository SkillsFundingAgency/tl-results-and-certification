using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ViewAll
{
    public class Then_Return_TwoRecord_ViewModel : When_ViewAll_Action_Called
    {
        private YourTlevelsViewModel mockresult;

        public override void Given()
        {
            mockresult = new YourTlevelsViewModel();

            TlevelLoader.GetYourTlevelsViewModel(Arg.Any<long>())
                .Returns(mockresult);
        }

        [Fact]
        public void Then_GetAllTlevelsByUkprnAsync_Is_Called()
        {
            TlevelLoader.Received().GetYourTlevelsViewModel(Arg.Any<long>());
        }

        [Fact(Skip ="TODO: Bookmark codition")]
        public void Then_GetAllTlevelsByUkprnAsync_ViewModel_Return_Two_Rows()
        {
            var viewResult = Result.Result as ViewResult;
            var model = viewResult.Model as IList<YourTlevelsViewModel>;
            model.Count().Should().Be(2);
        }

        [Fact(Skip = "TODO: Bookmark codition")]
        public void Then_GetAllTlevelsByUkprnAsync_Index_Returns_Expected_ViewModel()
        {
            var viewResult = Result.Result as ViewResult;
            var model = viewResult.Model as IList<YourTlevelsViewModel>;

            model.Should().NotBeNullOrEmpty();
            model.Count().Should().Be(2);
            
            var expectedFirstModel = model.FirstOrDefault();
            //var actualModel = mockresult.FirstOrDefault();
            
            //expectedFirstModel.PathwayId.Should().Be(actualModel.PathwayId);
            //expectedFirstModel.StatusId.Should().Be(actualModel.StatusId);
            //expectedFirstModel.TlevelTitle.Should().Be(actualModel.TlevelTitle);
        }
    }
}
