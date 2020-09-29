using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeSpecialismQuestionPost
{
    public class When_SpecialismQuestion_Unchanged : TestSetup
    {
        public override void Given()
        {
            ViewModel.HasLearnerDecidedSpecialism = true;
        }

        [Fact]
        public void Then_Redirected_To_SelectSpecialisms()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.ChangeRegistrationSpecialisms);
        }
    }
}
