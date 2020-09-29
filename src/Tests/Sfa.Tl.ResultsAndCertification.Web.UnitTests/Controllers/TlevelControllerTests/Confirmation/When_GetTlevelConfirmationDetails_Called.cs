using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.Confirmation
{
    public class When_GetTlevelConfirmationDetails_Called : TestSetup
    {
        private TlevelConfirmationViewModel expectedResult;

        public override void Given()
        {
            Id = 1;
            TempData["IsRedirect"] = true;
            expectedResult = new TlevelConfirmationViewModel { PathwayId = 1, ShowMoreTlevelsToReview = true, TlevelConfirmationText = "Success", TlevelTitle = "Title", IsQueried = true };

            TlevelLoader.GetTlevelConfirmationDetailsAsync(ukprn, Id)
                .Returns(expectedResult);
        }

        [Fact]
        public void Then_Called_Expected_Methods()
        {
            TlevelLoader.Received(1).GetTlevelConfirmationDetailsAsync(ukprn, Id);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as TlevelConfirmationViewModel;

            model.Should().NotBeNull();
            model.PathwayId.Should().Be(expectedResult.PathwayId);
            model.ShowMoreTlevelsToReview.Should().Be(expectedResult.ShowMoreTlevelsToReview);
            model.IsQueried.Should().Be(expectedResult.IsQueried);
            model.TlevelConfirmationText.Should().Be(expectedResult.TlevelConfirmationText);
            model.TlevelTitle.Should().Be(expectedResult.TlevelTitle);
        }
    }
}
