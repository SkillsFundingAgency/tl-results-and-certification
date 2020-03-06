using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SelectToReview;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.SelectToReviewPost
{
    public class Then_On_ModelState_Invalid_GetTlevelsToReviewByUkprnAsync_Is_Called : When_SelecctToReview_Get_Action_Is_Called
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
        public void Then_GetTlevelsToReviewByUkprnAsync_Is_Called()
        {
            TlevelLoader.Received().GetTlevelsToReviewByUkprnAsync(ukprn);
        }
    }
}
