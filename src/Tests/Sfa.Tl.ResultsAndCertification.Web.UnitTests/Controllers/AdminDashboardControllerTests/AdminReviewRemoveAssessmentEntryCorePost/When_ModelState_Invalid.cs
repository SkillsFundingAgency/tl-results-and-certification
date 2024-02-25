using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewRemoveAssessmentEntryCorePost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private const string ErrorKey = "AdminReviewRemoveAssessmentEntry";
        private int RegistrationPathwayId = 1;

        public override void Given()
        {
            ReviewRemoveCoreAssessmentEntryViewModel = new()
            {
                PathwayAssessmentViewModel = new() { 
                    RegistrationPathwayId = 1, 
                }
            };

            Controller.ModelState.AddModelError(ErrorKey, AdminReviewRemoveAssessmentEntry.Validation_Contact_Name_Blank_Text);
            Controller.ModelState.AddModelError(ErrorKey, AdminReviewRemoveAssessmentEntry.Validation_Date_When_Change_Requested_Blank_Text);
            Controller.ModelState.AddModelError(ErrorKey, AdminReviewRemoveAssessmentEntry.Validation_Date_When_Change_Requested_Invalid_Text);
            Controller.ModelState.AddModelError(ErrorKey, AdminReviewRemoveAssessmentEntry.Validation_Date_When_Change_Requested_Future_Date_Text);
            Controller.ModelState.AddModelError(ErrorKey, AdminReviewRemoveAssessmentEntry.Validation_Reason_For_Change_Blank_Text);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.DidNotReceive().ProcessRemoveAssessmentEntry(ReviewRemoveCoreAssessmentEntryViewModel);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var model = Result.ShouldBeViewResult<AdminReviewRemoveCoreAssessmentEntryViewModel>();

            model.Should().NotBeNull();
            model.PathwayAssessmentViewModel.RegistrationPathwayId.Should().Be(RegistrationPathwayId);


            ModelStateDictionary modelState = Controller.ViewData.ModelState;
            modelState.Should().HaveCount(1);
            modelState.Should().ContainKey(ErrorKey);
            modelState[ErrorKey].Errors[0].ErrorMessage.Should().Be(AdminReviewRemoveAssessmentEntry.Validation_Contact_Name_Blank_Text);
            modelState[ErrorKey].Errors[1].ErrorMessage.Should().Be(AdminReviewRemoveAssessmentEntry.Validation_Date_When_Change_Requested_Blank_Text);
            modelState[ErrorKey].Errors[2].ErrorMessage.Should().Be(AdminReviewRemoveAssessmentEntry.Validation_Date_When_Change_Requested_Invalid_Text);
            modelState[ErrorKey].Errors[3].ErrorMessage.Should().Be(AdminReviewRemoveAssessmentEntry.Validation_Date_When_Change_Requested_Future_Date_Text);
            modelState[ErrorKey].Errors[4].ErrorMessage.Should().Be(AdminReviewRemoveAssessmentEntry.Validation_Reason_For_Change_Blank_Text);
        }
    }
}
