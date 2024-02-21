using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.RemoveAssessmentEntrySpecialismPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private AdminRemoveSpecialismAssessmentEntryViewModel _viewModel;
        private const string ErrorKey = "RemoveAssessmentEntryCore";

        public override void Given()
        {
            _viewModel = CreateViewModel();
            Controller.ModelState.AddModelError(ErrorKey, RemoveAssessmentEntrySpecialism.Validation_Message);
        }

        public async override Task When()
        {
            Result = await Controller.RemoveAssessmentEntrySpecialismAsync(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.DidNotReceive().RemoveAsync<AdminRemoveSpecialismAssessmentEntryViewModel>(CacheKey);
            CacheService.DidNotReceive().SetAsync(CacheKey, _viewModel);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var model = Result.ShouldBeViewResult<AdminRemoveSpecialismAssessmentEntryViewModel>();
            model.Should().BeEquivalentTo(_viewModel);

            ModelStateDictionary modelState = Controller.ViewData.ModelState;
            modelState.Should().HaveCount(1);
            modelState.Should().ContainKey(ErrorKey);
            modelState[ErrorKey].Errors[0].ErrorMessage.Should().Be(RemoveAssessmentEntrySpecialism.Validation_Message);
        }
    }
}