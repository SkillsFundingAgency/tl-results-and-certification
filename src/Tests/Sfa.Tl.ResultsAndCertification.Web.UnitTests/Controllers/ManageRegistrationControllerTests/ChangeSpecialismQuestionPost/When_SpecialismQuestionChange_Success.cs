using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeSpecialismQuestionPost
{
    public class When_SpecialismQuestionChange_Success : TestSetup
    {
        public override void Given()
        {
            MockResult.IsSuccess = true;
            MockResult.IsModified = true;

            RegistrationLoader.ProcessSpecialismQuestionChangeAsync(AoUkprn, ViewModel).Returns(MockResult);
        }

        [Fact]
        public void Then_Redirected_To_ChangeRegistrationConfirmation()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.ChangeRegistrationConfirmation);
        }
    }
}
