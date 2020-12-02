using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;
using AssessmentContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.RemoveCoreAssessmentEntryPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            ViewModel = new AssessmentEntryDetailsViewModel
            {
                ProfileId = ProfileId,
                AssessmentId = 5,
                AssessmentSeriesName = "Summer 2021",
                CanRemoveAssessmentEntry = null
            };
            Controller.ModelState.AddModelError(nameof(AssessmentEntryDetailsViewModel.CanRemoveAssessmentEntry), AssessmentContent.RemoveCoreAssessmentEntry.Select_RemoveCoreAssessment_Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(AssessmentEntryDetailsViewModel));

            var model = viewResult.Model as AssessmentEntryDetailsViewModel;
            model.Should().NotBeNull();

            model.ProfileId.Should().Be(ViewModel.ProfileId);
            model.AssessmentId.Should().Be(ViewModel.AssessmentId);
            model.AssessmentSeriesName.Should().Be(ViewModel.AssessmentSeriesName);
            model.CanRemoveAssessmentEntry.Should().BeNull();

            Controller.ViewData.ModelState.ContainsKey(nameof(AssessmentEntryDetailsViewModel.CanRemoveAssessmentEntry)).Should().BeTrue();
            var modelState = Controller.ViewData.ModelState[nameof(AssessmentEntryDetailsViewModel.CanRemoveAssessmentEntry)];
            modelState.Errors[0].ErrorMessage.Should().Be(AssessmentContent.RemoveCoreAssessmentEntry.Select_RemoveCoreAssessment_Validation_Message);

            // Backlink
            var backLink = model.BackLink;
            backLink.RouteName.Should().Be(RouteConstants.AssessmentDetails);
            backLink.RouteAttributes.Count.Should().Be(1);
            backLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeValue);
            routeValue.Should().Be(ProfileId.ToString());
        }
    }
}
