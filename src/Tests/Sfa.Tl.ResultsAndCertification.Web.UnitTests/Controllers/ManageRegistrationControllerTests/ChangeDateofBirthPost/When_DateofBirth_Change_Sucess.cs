using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeDateofBirthPost
{
    public class When_DateofBirth_Change_Sucess : TestSetup
    {
        public override void Given()
        {
            MockResult.IsSuccess = true;
            MockResult.IsModified = true;

            RegistrationLoader.ProcessDateofBirthChangeAsync(AoUkprn, ViewModel)
                .Returns(MockResult);
        }

        [Fact]
        public void Then_Redirected_To_ChangeRegistrationConfirmation()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.ChangeRegistrationConfirmation);
        }
    }
}
