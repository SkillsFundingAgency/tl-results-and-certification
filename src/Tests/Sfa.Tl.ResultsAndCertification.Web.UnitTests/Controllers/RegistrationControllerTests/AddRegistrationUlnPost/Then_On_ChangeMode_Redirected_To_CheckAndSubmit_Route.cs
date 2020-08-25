using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationUlnPost
{
    public class Then_On_ChangeMode_Redirected_To_CheckAndSubmit_Route : When_AddRegistrationUln_Action_Is_Called
    {
        public override void Given()
        {
            UlnViewModel = new UlnViewModel { Uln = "1234567890", IsChangeMode = true };
        }

        [Fact]
        public void Then_Redirected_To_AddRegistrationCheckAndSubmit_Route()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.AddRegistrationCheckAndSubmit);
        }
    }
}
