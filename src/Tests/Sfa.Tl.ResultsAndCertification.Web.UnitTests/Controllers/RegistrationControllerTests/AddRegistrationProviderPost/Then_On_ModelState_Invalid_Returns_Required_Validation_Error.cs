using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;
using SelectProviderContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.SelectProvider;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationProviderPost
{
    public class Then_On_ModelState_Invalid_Returns_Required_Validation_Error : When_AddRegistrationProviderAsync_Post_Action_Is_Called
    {
        private SelectProviderViewModel _selectProviderViewModel;
        public override void Given()
        {
            SelectProviderViewModel = new SelectProviderViewModel();

            Controller.ModelState.AddModelError("SelectedProviderId", SelectProviderContent.Validation_Select_Provider_Required);

            _selectProviderViewModel = new SelectProviderViewModel { ProvidersSelectList = new List<SelectListItem> { new SelectListItem { Text = "Hello", Value = "1" } } };
           
            RegistrationLoader.GetRegisteredTqAoProviderDetailsAsync(Ukprn).Returns(_selectProviderViewModel);
        }

        [Fact]
        public void Then_Expected_Required_Error_Message_Is_Returned()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(SelectProviderViewModel));

            Controller.ViewData.ModelState.ContainsKey(nameof(SelectProviderViewModel.SelectedProviderId)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(SelectProviderViewModel.SelectedProviderId)];
            modelState.Errors[0].ErrorMessage.Should().Be(SelectProviderContent.Validation_Select_Provider_Required);
        }
    }
}
