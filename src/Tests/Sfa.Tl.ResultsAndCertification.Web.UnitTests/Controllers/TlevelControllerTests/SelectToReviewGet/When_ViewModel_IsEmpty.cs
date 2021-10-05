using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SelectToReview;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.SelectToReviewGet
{
    public class When_ViewModel_IsEmpty : TestSetup
    {
        public override void Given()
        {
            var mockresult = new SelectToReviewPageViewModel();
            TlevelLoader.GetTlevelsToReviewByUkprnAsync(Arg.Any<long>())
                .Returns(mockresult);
        }

        [Fact]
        public void Then_Called_Expected_Method()
        {
            TlevelLoader.Received().GetTlevelsToReviewByUkprnAsync(AoUkprn);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
