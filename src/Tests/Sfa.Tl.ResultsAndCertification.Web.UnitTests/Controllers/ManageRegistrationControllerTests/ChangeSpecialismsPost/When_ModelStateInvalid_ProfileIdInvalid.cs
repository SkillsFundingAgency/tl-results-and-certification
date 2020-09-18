using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;
using SelectSpecialisms = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.SelectSpecialisms;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeSpecialismsPost
{
    public class When_ModelStateInvalid_ProfileIdInvalid : TestSetup
    {
        private readonly ChangeSpecialismViewModel mockChangeSpecialismViewModel = null;

        public override void Given()
        {
            ViewModel.PathwaySpecialisms = new ViewModel.PathwaySpecialismsViewModel();
            RegistrationLoader.GetRegistrationProfileAsync<ChangeSpecialismViewModel>(AoUkprn, ViewModel.ProfileId).Returns(mockChangeSpecialismViewModel);
            Controller.ModelState.AddModelError("HasSpecialismSelected", SelectSpecialisms.Validation_Select_Specialism_Required_Message);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
