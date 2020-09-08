using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeProviderGet
{
    public class When_ProfileId_Valid : TestSetup
    {
        private SelectProviderViewModel _selectProviderViewModel;
        private ChangeProviderViewModel mockresult = null;
        public override void Given()
        {
            mockresult = new ChangeProviderViewModel
            {
                ProfileId = 1,
                SelectedProviderUkprn = "12345678"
            };

            _selectProviderViewModel = new SelectProviderViewModel { ProvidersSelectList = new List<SelectListItem> { new SelectListItem { Text = "Hello", Value = "1" } } };

            RegistrationLoader.GetRegisteredTqAoProviderDetailsAsync(AoUkprn).Returns(_selectProviderViewModel);

            RegistrationLoader.GetRegistrationProfileAsync<ChangeProviderViewModel>(AoUkprn, ProfileId).Returns(mockresult);
        }

        [Fact]
        public void Then_Recieved_Call_To_GetProviderDetails()
        {
            RegistrationLoader.Received(1).GetRegisteredTqAoProviderDetailsAsync(AoUkprn);
        }

        [Fact]
        public void Then_Recieved_Call_To_GetProfile()
        {
            RegistrationLoader.Received(1).GetRegistrationProfileAsync<ChangeProviderViewModel>(AoUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ChangeProviderViewModel));

            var model = viewResult.Model as ChangeProviderViewModel;
            model.Should().NotBeNull();

            model.ProfileId.Should().Be(mockresult.ProfileId);
            model.SelectedProviderUkprn.Should().Be(mockresult.SelectedProviderUkprn);
            model.ProvidersSelectList.Should().NotBeNull();
            model.ProvidersSelectList.Count.Should().Be(_selectProviderViewModel.ProvidersSelectList.Count);

            var backLink = model.BackLink;
            backLink.RouteName.Should().Be(RouteConstants.RegistrationDetails);
            backLink.RouteAttributes.Count.Should().Be(1);
            backLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeValue);
            routeValue.Should().Be(mockresult.ProfileId.ToString());
        }
    }
}
