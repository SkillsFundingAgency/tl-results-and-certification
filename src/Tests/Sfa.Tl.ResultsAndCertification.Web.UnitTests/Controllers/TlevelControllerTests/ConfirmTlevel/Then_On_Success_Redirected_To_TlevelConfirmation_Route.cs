using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ConfirmTlevel
{
    public class Then_On_Success_Redirected_To_TlevelConfirmation_Route : When_ConfirmTlevel_Action_Is_Called
    {
        private readonly int pathwayId = 99;

        public override void Given()
        {

            InputModel = new ConfirmTlevelViewModel { PathwayStatusId = (int)TlevelReviewStatus.AwaitingConfirmation, PathwayId = pathwayId };
            TlevelLoader.ConfirmTlevelAsync(InputModel).Returns(true);
        }

        [Fact]
        public void Then_ModelState_Valid_Redirected_To_TlevelConfirmation()
        {
            var routeName = (Result.Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.TlevelConfirmation);
        }
    }
}
