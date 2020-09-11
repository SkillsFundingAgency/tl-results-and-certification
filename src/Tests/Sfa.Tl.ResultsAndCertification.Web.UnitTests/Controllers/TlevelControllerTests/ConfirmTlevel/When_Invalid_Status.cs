using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ConfirmTlevel
{
    public class When_Invalid_Status : TestSetup
    {
        public override void Given()
        {
            InputModel = new ConfirmTlevelViewModel { PathwayStatusId = (int)TlevelReviewStatus.Confirmed };
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result.Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
