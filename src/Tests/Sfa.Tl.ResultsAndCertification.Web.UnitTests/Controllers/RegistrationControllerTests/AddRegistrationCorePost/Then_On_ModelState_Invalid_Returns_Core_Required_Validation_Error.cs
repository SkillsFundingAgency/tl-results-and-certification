using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;
using SelectCoreContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.SelectCore;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationCorePost
{
    public class Then_On_ModelState_Invalid_Returns_Core_Required_Validation_Error : When_AddRegistrationCoreAsync_Action_Is_Called
    {
        private SelectProviderViewModel _selectProviderViewModel;
        private SelectCoreViewModel _selectCoreViewModel;
        private long _providerUkprn = 987654321;

        public override void Given()
        {
            SelectCoreViewModel = new SelectCoreViewModel();
            Controller.ModelState.AddModelError("SelectedCoreCode", SelectCoreContent.Validation_Select_Core_Required);

            _selectProviderViewModel = new SelectProviderViewModel { SelectedProviderId = _providerUkprn.ToString(), ProvidersSelectList = new List<SelectListItem> { new SelectListItem { Text = "Test Provider", Value = _providerUkprn.ToString() } } };
            _selectCoreViewModel = new SelectCoreViewModel { SelectedCoreCode = "123", CoreSelectList = new List<SelectListItem> { new SelectListItem { Text = "Education", Value = "123" } } };

            var cacheResult = new RegistrationViewModel
            {
                SelectProvider = _selectProviderViewModel,
                SelectCore = _selectCoreViewModel
            };

            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(cacheResult);
            RegistrationLoader.GetRegisteredProviderPathwayDetailsAsync(Ukprn, _providerUkprn).Returns(_selectCoreViewModel);
        }

        [Fact]
        public void Then_Expected_Required_Error_Message_Is_Returned()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(SelectCoreViewModel));

            Controller.ViewData.ModelState.ContainsKey(nameof(SelectCoreViewModel.SelectedCoreCode)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(SelectCoreViewModel.SelectedCoreCode)];
            modelState.Errors[0].ErrorMessage.Should().Be(SelectCoreContent.Validation_Select_Core_Required);
        }
    }
}
