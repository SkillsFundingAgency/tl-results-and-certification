using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Xunit;
using AddAddressContent = Sfa.Tl.ResultsAndCertification.Web.Content.ProviderAddress;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddPostalAddressManualPost
{
    public class When_ModelState_IsInvalid : TestSetup
    {
        public override void Given()
        {
            ViewModel = new AddAddressManualViewModel
            {
                DepartmentName = "Finanace",
                OrganisationName = "Org name",
                AddressLine1 = "38",
                AddressLine2 = "Street Line",
                Town = "Birmingham"                
            };

            Controller.ModelState.AddModelError(nameof(AddAddressManualViewModel.Postcode), AddAddressContent.AddPostalAddressManual.Validation_Enter_Postcode);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            Controller.ViewData.ModelState.ContainsKey(nameof(AddAddressManualViewModel.Postcode)).Should().BeTrue();
            var modelState = Controller.ViewData.ModelState[nameof(AddAddressManualViewModel.Postcode)];
            modelState.Errors[0].ErrorMessage.Should().Be(AddAddressContent.AddPostalAddressManual.Validation_Enter_Postcode);

            var model = (Result as ViewResult).Model as AddAddressManualViewModel;
            
            model.DepartmentName.Should().Be(ViewModel.DepartmentName);
            model.OrganisationName.Should().Be(ViewModel.OrganisationName);
            model.AddressLine1.Should().Be(ViewModel.AddressLine1);
            model.AddressLine2.Should().Be(ViewModel.AddressLine2);
            model.Town.Should().Be(ViewModel.Town);
            model.Postcode.Should().BeNull();
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AddAddressPostcode);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ShowPostcode, out string routeValue);
            routeValue.Should().Be("false");
        }
    }
}
