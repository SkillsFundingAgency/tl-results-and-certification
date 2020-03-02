using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SelectToReview;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.SelectToReviewGet
{
    public class Then_Redirected_On_Empty_ViewModel : When_SelecctToReview_Get_Action_Is_Called
    {
        public override void Given()
        {
            var mockresult = new SelectToReviewPageViewModel();
            TlevelLoader.GetTlevelsToReviewByUkprnAsync(Arg.Any<long>())
                .Returns(mockresult);
        }

        [Fact]
        public void Then_GetTlevelsToReviewByUkprnAsync_Is_Called()
        {
            TlevelLoader.Received().GetTlevelsToReviewByUkprnAsync(ukprn);
        }

        [Fact]
        public void Then_GetTlevelsToReviewByUkprnAsync_ViewModel_Return_Zero_Rows()
        {
            var actualRouteName = (Result.Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
