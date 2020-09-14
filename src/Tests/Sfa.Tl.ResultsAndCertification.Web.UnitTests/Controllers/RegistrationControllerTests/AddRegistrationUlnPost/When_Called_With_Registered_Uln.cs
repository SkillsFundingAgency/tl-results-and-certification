using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationUlnPost
{
    public class When_Called_With_Registered_Uln : TestSetup
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
        public void Then_Redirected_To_CannotBeRegistered()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.UlnCannotBeRegistered);
        }
    }
}
