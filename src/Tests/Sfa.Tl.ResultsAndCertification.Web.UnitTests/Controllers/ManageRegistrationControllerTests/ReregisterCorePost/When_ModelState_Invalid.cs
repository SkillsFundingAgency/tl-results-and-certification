using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;
using SelectCoreContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.SelectCore;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterCorePost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private ReregisterProviderViewModel _reregisterProviderViewModel;
        private SelectCoreViewModel _selectCoreViewModel;
        private long _providerUkprn = 987654321;

        public override void Given()
        {
            _reregisterProviderViewModel = new ReregisterProviderViewModel { ProfileId = ProfileId, SelectedProviderUkprn = _providerUkprn.ToString(), ProvidersSelectList = new List<SelectListItem> { new SelectListItem { Text = "Hello", Value = "1" } } };
            _selectCoreViewModel = new SelectCoreViewModel { SelectedCoreCode = "123", CoreSelectList = new List<SelectListItem> { new SelectListItem { Text = "Education", Value = "123" } } };

            var cacheResult = new ReregisterViewModel
            {
                ReregisterProvider = _reregisterProviderViewModel
            };

            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(cacheResult);
            RegistrationLoader.GetRegisteredProviderPathwayDetailsAsync(AoUkprn, _providerUkprn).Returns(_selectCoreViewModel);
            Controller.ModelState.AddModelError(nameof(ReregisterCoreViewModel.SelectedCoreCode), SelectCoreContent.Validation_Select_Core_Required);
        }

        [Fact]
        public void Then_Returns_Expected_ViewModel()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ReregisterCoreViewModel));

            var model = viewResult.Model as ReregisterCoreViewModel;
            model.Should().NotBeNull();
            model.CoreSelectList.Should().NotBeNull();
            model.CoreSelectList.Count.Should().Be(_selectCoreViewModel.CoreSelectList.Count);

            Controller.ViewData.ModelState.ContainsKey(nameof(ReregisterCoreViewModel.SelectedCoreCode)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(ReregisterCoreViewModel.SelectedCoreCode)];
            modelState.Errors[0].ErrorMessage.Should().Be(SelectCoreContent.Validation_Select_Core_Required);
        }
    }
}
