using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminAddSpecialismAppealOutcomePost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private AdminAddSpecialismAppealOutcomeViewModel _viewModel;
        private const string ErrorKey = "AdminAddSpecialismAppealOutcome";

        public override void Given()
        {
            _viewModel = CreateViewModel();
            Controller.ModelState.AddModelError(ErrorKey, AdminAddSpecialismAppealOutcome.Validation_Message);
        }

        public async override Task When()
        {
            Result = await Controller.AdminAddSpecialismAppealOutcomeAsync(_viewModel);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var model = Result.ShouldBeViewResult<AdminAddSpecialismAppealOutcomeViewModel>();
            model.Should().BeEquivalentTo(_viewModel);

            ModelStateDictionary modelState = Controller.ViewData.ModelState;
            modelState.Should().HaveCount(1);
            modelState.Should().ContainKey(ErrorKey);
            modelState[ErrorKey].Errors[0].ErrorMessage.Should().Be(AdminAddSpecialismAppealOutcome.Validation_Message);
        }
    }
}