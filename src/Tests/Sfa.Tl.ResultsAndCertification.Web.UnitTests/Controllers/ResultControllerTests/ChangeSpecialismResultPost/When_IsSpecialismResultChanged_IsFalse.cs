using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.ChangeSpecialismResultPost
{
    public class When_IsSpecialismResultChanged_IsFalse : TestSetup
    {
        private readonly bool? mockResult = false;

        public override void Given()
        {
            ResultLoader.IsSpecialismResultChangedAsync(AoUkprn, Arg.Any<ManageSpecialismResultViewModel>()).Returns(mockResult);
        }

        [Fact]
        public void Then_Redirected_To_ResultDetails()
        {
            Result.Should().NotBeNull();
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.ResultDetails);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
        }
    }
}
