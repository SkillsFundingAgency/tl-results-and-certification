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
    public class Then_On_ModelState_Invalid_Returns_Required_Validation_Error : When_AddRegistrationSpecialismQuestionAsync_Action_Is_Called
    {
        private RegistrationViewModel cacheResult;
        private SelectCoreViewModel _selectCoreViewModel;

        public override void Given()
        {
            SpecialismQuestionViewModel = new SpecialismQuestionViewModel();
            Controller.ModelState.AddModelError("HasLearnerDecidedSpecialism", SpecialismQuestionContent.Validation_Select_Yes_Required_Message);

            _selectCoreViewModel = new SelectCoreViewModel { SelectedCoreId = "123", CoreSelectList = new List<SelectListItem> { new SelectListItem { Text = "Education", Value = "123" } } };

            cacheResult = new RegistrationViewModel
            {
                SelectCore = _selectCoreViewModel
            };

            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Expected_Required_Error_Message_Is_Returned()
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
