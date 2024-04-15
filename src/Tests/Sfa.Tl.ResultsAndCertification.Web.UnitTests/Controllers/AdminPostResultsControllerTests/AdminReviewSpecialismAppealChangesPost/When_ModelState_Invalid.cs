using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminReviewSpecialismAppealChangesPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private AdminAppealSpecialismReviewChangesViewModel _viewModel;
        private const string ErrorKey = "AdminReviewSpecialismAppealChanges";

        private IActionResult _result;

        public override void Given()
        {
            _viewModel = CreateViewModel();
            Controller.ModelState.AddModelError(ErrorKey, AdminReviewSpecialismAppealChanges.Validation_Contact_Name_Blank_Text);
        }

        public async override Task When()
        {
            _result = await Controller.AdminReviewSpecialismAppealChangesAsync(_viewModel);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var model = _result.ShouldBeViewResult<AdminAppealSpecialismReviewChangesViewModel>();
            model.Should().BeEquivalentTo(_viewModel);

            ModelStateDictionary modelState = Controller.ViewData.ModelState;
            modelState.Should().HaveCount(1);
            modelState.Should().ContainKey(ErrorKey);
            modelState[ErrorKey].Errors[0].ErrorMessage.Should().Be(AdminReviewSpecialismAppealChanges.Validation_Contact_Name_Blank_Text);
        }
    }
}