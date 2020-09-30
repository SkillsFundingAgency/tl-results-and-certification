using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterProviderPost
{
    public class When_Success : TestSetup
    {
        private SelectProviderViewModel _selectProviderViewModel;
        public override void Given()
        {
            ViewModel = new ReregisterProviderViewModel { ProfileId = ProfileId, SelectedProviderUkprn = "1234567890" };
            _selectProviderViewModel = new SelectProviderViewModel { ProvidersSelectList = new List<SelectListItem> { new SelectListItem { Text = "Hello", Value = "1" } } };

            RegistrationLoader.GetRegisteredTqAoProviderDetailsAsync(AoUkprn).Returns(_selectProviderViewModel);
        }

        [Fact]
        public void Then_Redirected_To_ReregisterCore()
        {
            var route = Result as RedirectToRouteResult;
            var routeName = route.RouteName;
            routeName.Should().Be(RouteConstants.ReregisterCore);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
        }
    }
}
