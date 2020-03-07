using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ConfirmTlevel
{
    public class Then_IsEverythingCorrect_False_Rerouted_To_ReportIssue : When_ConfirmTlevel_Action_Is_Called
    {
        private readonly int pathwayId = 99;

        public override void Given()
        {

            InputModel = new VerifyTlevelViewModel { PathwayStatusId = (int)TlevelReviewStatus.AwaitingConfirmation, PathwayId = pathwayId, IsEverythingCorrect = false };
        }

        [Fact]
        public void Then_IsEveryThingCorrect_False_Is_Redirected_To_ReportIssue()
        {
            var routeName = (Result.Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.ReportTlevelIssue);
        }
    }
}
