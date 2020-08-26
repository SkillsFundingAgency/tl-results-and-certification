using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationUlnPost
{
    public class Then_On_Uln_Registered_Redirected_To_UlnCannotBeRegistered : When_AddRegistrationUln_Action_Is_Called
    {
        public override void Given()
        {
            var findUln = new UlnNotFoundViewModel
            {
                IsActive = true
            };

            UlnViewModel = new UlnViewModel { Uln = "1234567890" };
            RegistrationLoader.FindUlnAsync(Arg.Any<long>(), Arg.Any<long>()).Returns(findUln);
        }

        [Fact]
        public void Then_Redirected_To_CannotBeRegistered_Route()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.UlnCannotBeRegistered);
        }
    }
}
