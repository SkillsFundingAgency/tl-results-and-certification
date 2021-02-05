using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.ChangeCoreResultPost
{
    public class When_ChangeResult_Failed : TestSetup
    {
        public override void Given()
        {
            MockResult.IsSuccess = false;
            MockResult.Uln = 1234567891;
            MockResult.ProfileId = 1;
            ResultLoader.IsCoreResultChangedAsync(AoUkprn, Arg.Any<ManageCoreResultViewModel>()).Returns(true);
            ResultLoader.ChangeCoreResultAsync(AoUkprn, ViewModel).Returns(MockResult);
        }

        [Fact]
        public void Then_Redirected_To_ProblemWithService()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.ProblemWithService);
        }
    }
}
