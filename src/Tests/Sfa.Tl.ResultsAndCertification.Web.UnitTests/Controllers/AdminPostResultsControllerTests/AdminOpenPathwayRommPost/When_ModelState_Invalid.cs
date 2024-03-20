using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminOpenPathwayRommPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private AdminOpenPathwayRommViewModel _viewModel;
        private const string ErrorKey = "AdminOpenPathwayRomm";

        public override void Given()
        {
            _viewModel = CreateViewModel();
            Controller.ModelState.AddModelError(ErrorKey, AdminOpenPathwayRomm.Validation_Message);
        }

        public async override Task When()
        {
            Result = await Controller.AdminOpenPathwayRommAsync(_viewModel);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var model = Result.ShouldBeViewResult<AdminOpenPathwayRommViewModel>();
            model.Should().BeEquivalentTo(_viewModel);

            ModelStateDictionary modelState = Controller.ViewData.ModelState;
            modelState.Should().HaveCount(1);
            modelState.Should().ContainKey(ErrorKey);
            modelState[ErrorKey].Errors[0].ErrorMessage.Should().Be(AdminOpenPathwayRomm.Validation_Message);
        }
    }
}