using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminAddAppealOutcomeChangeGradeSpecialismPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private AdminAddAppealOutcomeChangeGradeSpecialismViewModel _viewModel;
        private const string ErrorKey = "AdminAddAppealOutcomeSpecialism";

        public override void Given()
        {
            _viewModel = CreateViewModel();
            Controller.ModelState.AddModelError(ErrorKey, AdminAddAppealOutcomeChangeGradeSpecialism.Validation_Message);
        }

        public async override Task When()
        {
            Result = await Controller.AdminAddAppealOutcomeChangeGradeSpecialismAsync(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminPostResultsLoader.Received(1).LoadAdminAddAppealOutcomeChangeGradeSpecialismGrades(_viewModel);
            CacheService.DidNotReceive().SetAsync(CacheKey, _viewModel);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var model = Result.ShouldBeViewResult<AdminAddAppealOutcomeChangeGradeSpecialismViewModel>();
            model.Should().BeEquivalentTo(_viewModel);

            ModelStateDictionary modelState = Controller.ViewData.ModelState;
            modelState.Should().HaveCount(1);
            modelState.Should().ContainKey(ErrorKey);
            modelState[ErrorKey].Errors[0].ErrorMessage.Should().Be(AdminAddAppealOutcomeChangeGradeSpecialism.Validation_Message);
        }
    }
}