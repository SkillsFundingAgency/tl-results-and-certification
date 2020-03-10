using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ConfirmTlevel
{
    public class Then_Not_Success_Redirected_To_Error_500 : When_ConfirmTlevel_Action_Is_Called
    {
        private readonly int pathwayId = 99;

        public override void Given()
        {

            InputModel = new VerifyTlevelViewModel { PathwayStatusId = (int)TlevelReviewStatus.AwaitingConfirmation, PathwayId = pathwayId };
            TlevelLoader.VerifyTlevelAsync(InputModel).Returns(false);
        }

        [Fact]
        public void Then_Status_Update_Fail_Redirected_To_Error_500()
        {
            var routeName = (Result.Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be("error/500");
        }
    }
}
