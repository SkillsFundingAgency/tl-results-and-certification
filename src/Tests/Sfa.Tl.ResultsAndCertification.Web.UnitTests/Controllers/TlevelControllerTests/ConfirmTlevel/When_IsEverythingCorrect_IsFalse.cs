using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ConfirmTlevel
{
    public class When_IsEverythingCorrect_IsFalse : TestSetup
    {
        private readonly int pathwayId = 99;

        public override void Given()
        {
            InputModel = new ConfirmTlevelViewModel { PathwayStatusId = (int)TlevelReviewStatus.AwaitingConfirmation, PathwayId = pathwayId, IsEverythingCorrect = false };
        }

        [Fact]
        public void Then_Redirected_To_QueryTlevelDetails()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.QueryTlevelDetails);
        }
    }
}
