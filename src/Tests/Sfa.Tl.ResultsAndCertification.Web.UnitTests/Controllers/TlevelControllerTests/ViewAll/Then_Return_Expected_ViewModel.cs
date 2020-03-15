using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ViewAll
{
    public class Then_Return_Expected_ViewModel : When_ViewAll_Action_Called
    {
        int expectedPathwayId = 11;
        string expectedTlevelTitle = "Route: Pathway";

        public override void Given()
        {
            var mockresult = new YourTlevelsViewModel 
            {
                IsAnyReviewPending = true,
                ConfirmedTlevels = new List<YourTlevelViewModel> 
                {
                    new YourTlevelViewModel { PathwayId = expectedPathwayId, TlevelTitle = expectedTlevelTitle }
                },
                QueriedTlevels = new List<YourTlevelViewModel>()
            };

            TlevelLoader.GetYourTlevelsViewModel(Arg.Any<long>())
                .Returns(mockresult);
        }

        [Fact]
        public void Then_GetAllTlevelsByUkprnAsync_Is_Called()
        {
            TlevelLoader.Received().GetYourTlevelsViewModel(Arg.Any<long>());
        }

        [Fact]
        public void Then_GetAllTlevelsByUkprnAsync_ViewModel_Return_ExpectedResults()
        {
            var viewResult = Result.Result as ViewResult;
            var model = viewResult.Model as YourTlevelsViewModel;

            model.IsAnyReviewPending.Should().BeTrue();
            model.ConfirmedTlevels.Should().NotBeNull();
            model.ConfirmedTlevels.Count().Should().Be(1);
            model.ConfirmedTlevels.First().PathwayId.Should().Be(expectedPathwayId);
            model.ConfirmedTlevels.First().TlevelTitle.Should().Be(expectedTlevelTitle);
            model.QueriedTlevels.Should().NotBeNull();
            model.QueriedTlevels.Should().BeEmpty();
        }
    }
}
