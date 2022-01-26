using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.AddSpecialismResultPost
{
    public class When_AddResult_Failed : TestSetup
    {
        public override void Given()
        {
            MockResult.IsSuccess = false;
            MockResult.Uln = 1234567891;
            MockResult.ProfileId = 1;
            ResultLoader.AddSpecialismResultAsync(AoUkprn, ViewModel).Returns(MockResult);
        }

        [Fact]
        public void Then_Redirected_To_ProblemWithService()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.ProblemWithService);
        }
    }
}
