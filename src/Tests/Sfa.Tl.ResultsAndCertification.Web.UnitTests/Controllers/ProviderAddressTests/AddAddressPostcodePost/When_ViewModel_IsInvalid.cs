using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Xunit;
using AddAddressContent = Sfa.Tl.ResultsAndCertification.Web.Content.ProviderAddress;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddAddressPostcodePost
{
    public class When_ViewModel_IsInvalid : TestSetup
    {
        public override void Given() 
        {
            ViewModel = new AddAddressPostcodeViewModel();
            Controller.ModelState.AddModelError(nameof(AddAddressPostcodeViewModel.Postcode), AddAddressContent.AddAddressPostcode.Validation_Enter_Postcode);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            Controller.ViewData.ModelState.ContainsKey(nameof(AddAddressPostcodeViewModel.Postcode)).Should().BeTrue();
            var modelState = Controller.ViewData.ModelState[nameof(AddAddressPostcodeViewModel.Postcode)];
            modelState.Errors[0].ErrorMessage.Should().Be(AddAddressContent.AddAddressPostcode.Validation_Enter_Postcode);

            var model = (Result as ViewResult).Model as AddAddressPostcodeViewModel;
            model.BackLink.Should().NotBeNull();
            model.Postcode.Should().BeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.ManagePostalAddress);
            model.BackLink.RouteAttributes.Count.Should().Be(0);
        }
    }
}
