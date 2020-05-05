using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SelectToReview;
using System.Collections.Generic;
using Xunit;


namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.SelectToReviewGet
{
    public class Then_On_Id_Passed_In_Route_Returns_SelectedPathwayId : When_SelecctToReview_Get_Action_Is_Called
    {
        private SelectToReviewPageViewModel mockresult;

        public override void Given()
        {
            selectedPathwayId = 99;

            mockresult = new SelectToReviewPageViewModel
            {
                TlevelsToReview = new List<TlevelToReviewViewModel>
                {
                    new TlevelToReviewViewModel { PathwayId = 1, TlevelTitle = "Route1: Pathway1"},
                }
            };

            TlevelLoader.GetTlevelsToReviewByUkprnAsync(Arg.Any<long>())
                .Returns(mockresult);
        }

        [Fact]
        public void Then_Expected_SelectedPathwayId_Is_Returned()
        {
            var viewResult = Result.Result as ViewResult;
            var model = viewResult.Model as SelectToReviewPageViewModel;

            model.Should().NotBeNull();
            model.SelectedPathwayId.Should().Be(selectedPathwayId);
        }
    }
}
