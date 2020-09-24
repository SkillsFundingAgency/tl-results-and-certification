using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;
using SelectProviderContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.SelectProvider;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterProviderPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private SelectProviderViewModel _selectProviderViewModel;

        public override void Given()
        {
            _selectProviderViewModel = new SelectProviderViewModel { ProvidersSelectList = new List<SelectListItem> { new SelectListItem { Text = "Hello", Value = "1" } } };

            RegistrationLoader.GetRegisteredTqAoProviderDetailsAsync(AoUkprn).Returns(_selectProviderViewModel);

            Controller.ModelState.AddModelError(nameof(ReregisterProviderViewModel.SelectedProviderUkprn), SelectProviderContent.Validation_Select_Provider_Required);
        }

        [Fact]
        public void Then_Returns_Expected_ViewModel()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ReregisterProviderViewModel));

            var model = viewResult.Model as ReregisterProviderViewModel;
            model.Should().NotBeNull();
            model.ProvidersSelectList.Should().NotBeNull();
            model.ProvidersSelectList.Count.Should().Be(_selectProviderViewModel.ProvidersSelectList.Count);

            Controller.ViewData.ModelState.ContainsKey(nameof(ReregisterProviderViewModel.SelectedProviderUkprn)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(ReregisterProviderViewModel.SelectedProviderUkprn)];
            modelState.Errors[0].ErrorMessage.Should().Be(SelectProviderContent.Validation_Select_Provider_Required);
        }
    }
}
