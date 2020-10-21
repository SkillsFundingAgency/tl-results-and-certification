using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SelectToReview;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.SelectToReviewPost
{
    public class When_ModelState_IsValid : TestSetup
    {
        private SelectToReviewPageViewModel mockresult;
        private readonly int selectedPathwayId = 11;

        public override void Given()
        {
            InputModel = new SelectToReviewPageViewModel { SelectedPathwayId = selectedPathwayId };
            Controller.ModelState.AddModelError("SelectedPathwayId", "Please select a T level.");

            mockresult = new SelectToReviewPageViewModel
            {
                SelectedPathwayId = selectedPathwayId,
                TlevelsToReview = new List<TlevelToReviewViewModel> { }
            };

            TlevelLoader.GetTlevelsToReviewByUkprnAsync(Arg.Any<long>())
                .Returns(mockresult);
        }

        [Fact]
        public void Then_Called_Expected_Method()
        {
            TlevelLoader.Received().GetTlevelsToReviewByUkprnAsync(ukprn);
        }
    }
}
