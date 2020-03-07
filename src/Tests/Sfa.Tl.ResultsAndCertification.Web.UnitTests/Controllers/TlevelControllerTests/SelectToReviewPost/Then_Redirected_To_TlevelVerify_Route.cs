using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SelectToReview;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.SelectToReviewPost
{
    public class Then_Redirected_To_TlevelVerify_Route : When_SelecctToReview_Get_Action_Is_Called
    {
        private readonly int selectedPathwayId = 11;
        public override void Given()
        {
            InputModel = new SelectToReviewPageViewModel { SelectedPathwayId = selectedPathwayId };
        }

        [Fact]
        public void Then_Redirected_To_Tlevel_Verify_Route()
        {
            var route = (Result.Result as RedirectToRouteResult);
            route.Should().NotBeNull();

            var actualRouteName = route.RouteName;
            var routeParam = route.RouteValues["id"];

            actualRouteName.Should().Be(RouteConstants.VerifyTlevel);
            routeParam.Should().NotBeNull();
            routeParam.ToString().Should().Be(selectedPathwayId.ToString());
        }
    }
}
