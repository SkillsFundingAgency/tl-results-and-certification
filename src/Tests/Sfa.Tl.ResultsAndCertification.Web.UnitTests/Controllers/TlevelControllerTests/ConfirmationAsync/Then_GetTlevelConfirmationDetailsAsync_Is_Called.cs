using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ConfirmationAsync
{
    public class Then_GetTlevelConfirmationDetailsAsync_Is_Called : When_ConfirmationAsync_Is_Called
    {
        private TlevelConfirmationViewModel expectedResult;

        public override void Given()
        {
            Id = 1;
            TempData["IsRedirect"] = true;
            expectedResult = new TlevelConfirmationViewModel { PathwayId = 1, ShowMoreTlevelsToReview = true, TlevelConfirmationText = "Success", TlevelTitle = "Title" };

            TlevelLoader.GetTlevelConfirmationDetailsAsync(ukprn, Id)
                .Returns(expectedResult);
        }

        [Fact]
        public void Then_GetTlevelConfirmationDetailsAsync_Method_Is_Called()
        {
            TlevelLoader.Received(1).GetTlevelConfirmationDetailsAsync(ukprn, Id);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            var viewResult = Result.Result as ViewResult;
            var model = viewResult.Model as TlevelConfirmationViewModel;

            model.Should().NotBeNull();
            model.PathwayId.Should().Be(expectedResult.PathwayId);
            model.ShowMoreTlevelsToReview.Should().Be(expectedResult.ShowMoreTlevelsToReview);
            model.TlevelConfirmationText.Should().Be(expectedResult.TlevelConfirmationText);
            model.TlevelTitle.Should().Be(expectedResult.TlevelTitle);
        }
    }
}
