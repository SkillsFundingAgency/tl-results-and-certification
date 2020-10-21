using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;
using SpecialismQuestionContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.SpecialismQuestion;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationSpecialismQuestionPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private RegistrationViewModel cacheResult;
        private SelectCoreViewModel _selectCoreViewModel;

        public override void Given()
        {
            SpecialismQuestionViewModel = new SpecialismQuestionViewModel();
            Controller.ModelState.AddModelError("HasLearnerDecidedSpecialism", SpecialismQuestionContent.Validation_Select_Yes_Required_Message);

            _selectCoreViewModel = new SelectCoreViewModel { SelectedCoreCode = "123", CoreSelectList = new List<SelectListItem> { new SelectListItem { Text = "Education", Value = "123" } } };

            cacheResult = new RegistrationViewModel
            {
                SelectCore = _selectCoreViewModel
            };

            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(SpecialismQuestionViewModel));

            Controller.ViewData.ModelState.ContainsKey(nameof(SpecialismQuestionViewModel.HasLearnerDecidedSpecialism)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(SpecialismQuestionViewModel.HasLearnerDecidedSpecialism)];
            modelState.Errors[0].ErrorMessage.Should().Be(SpecialismQuestionContent.Validation_Select_Yes_Required_Message);
        }
    }
}
