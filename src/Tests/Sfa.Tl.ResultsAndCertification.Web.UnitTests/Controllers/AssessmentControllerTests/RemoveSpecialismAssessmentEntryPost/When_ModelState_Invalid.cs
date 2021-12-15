using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;
using AssessmentContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.RemoveSpecialismAssessmentEntryPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private RemoveSpecialismAssessmentEntryViewModel _mockresult = null;

        public override void Given()
        {
            ViewModel = new RemoveSpecialismAssessmentEntryViewModel
            {
                ProfileId = 1,
                AssessmentSeriesName = "Summer 2021",
                CanRemoveAssessmentEntry = null
            };

            _mockresult = new RemoveSpecialismAssessmentEntryViewModel
            {
                ProfileId = 1,
                AssessmentSeriesName = "Summer 2021",
            };

            AssessmentLoader.GetRemoveSpecialismAssessmentEntriesAsync(AoUkprn, ViewModel.ProfileId, _mockresult.SpecialismAssessmentIds).Returns(_mockresult);
            Controller.ModelState.AddModelError(nameof(RemoveSpecialismAssessmentEntryViewModel.CanRemoveAssessmentEntry), AssessmentContent.RemoveSpecialismAssessmentEntries.Select_Option_To_Remove_Validation_Text);
        }

        [Fact]
        public void Then_Expected_Results_Returned()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(RemoveSpecialismAssessmentEntryViewModel));

            var model = viewResult.Model as RemoveSpecialismAssessmentEntryViewModel;
            model.Should().NotBeNull();

            model.ProfileId.Should().Be(ViewModel.ProfileId);
            model.AssessmentSeriesName.Should().Be(ViewModel.AssessmentSeriesName);
            model.CanRemoveAssessmentEntry.Should().BeNull();

            Controller.ViewData.ModelState.ContainsKey(nameof(RemoveSpecialismAssessmentEntryViewModel.CanRemoveAssessmentEntry)).Should().BeTrue();
            var modelState = Controller.ViewData.ModelState[nameof(RemoveSpecialismAssessmentEntryViewModel.CanRemoveAssessmentEntry)];
            modelState.Errors[0].ErrorMessage.Should().Be(AssessmentContent.RemoveCoreAssessmentEntry.Select_Option_To_Remove_Validation_Text);

            // Backlink
            var backLink = model.BackLink;
            backLink.RouteName.Should().Be(RouteConstants.AssessmentDetails);
            backLink.RouteAttributes.Count.Should().Be(1);
            backLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeValue);
            routeValue.Should().Be(ProfileId.ToString());
        }
    }
}
