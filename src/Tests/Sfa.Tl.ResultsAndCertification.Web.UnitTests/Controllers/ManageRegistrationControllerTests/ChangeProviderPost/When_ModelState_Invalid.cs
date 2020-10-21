using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;
using SelectProviderContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.SelectProvider;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeProviderPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private SelectProviderViewModel _selectProviderViewModel;
        public override void Given()
        {
            _selectProviderViewModel = new SelectProviderViewModel { ProvidersSelectList = new List<SelectListItem> { new SelectListItem { Text = "Hello", Value = "1" } } };

            RegistrationLoader.GetRegisteredTqAoProviderDetailsAsync(AoUkprn).Returns(_selectProviderViewModel);

            Controller.ModelState.AddModelError("SelectedProviderUkprn", SelectProviderContent.Validation_Select_Provider_Required);
        }

        [Fact]
        public void Then_Returns_Expected_ViewModel()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ChangeProviderViewModel));

            var model = viewResult.Model as ChangeProviderViewModel;
            model.Should().NotBeNull();
            model.ProvidersSelectList.Should().NotBeNull();
            model.ProvidersSelectList.Count.Should().Be(_selectProviderViewModel.ProvidersSelectList.Count);

            Controller.ViewData.ModelState.ContainsKey(nameof(SelectProviderViewModel.SelectedProviderUkprn)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(SelectProviderViewModel.SelectedProviderUkprn)];
            modelState.Errors[0].ErrorMessage.Should().Be(SelectProviderContent.Validation_Select_Provider_Required);

        }
    }
}
