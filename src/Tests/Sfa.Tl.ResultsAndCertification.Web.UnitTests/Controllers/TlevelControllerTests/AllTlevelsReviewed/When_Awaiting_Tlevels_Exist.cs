using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.AllTlevelsReviewed
{
    public class When_Awaiting_Tlevels_Exist : TestSetup
    {
        public override void Given()
        {
            var mockresult = new List<YourTlevelViewModel>
            {
                    new YourTlevelViewModel { PathwayId = 1, TlevelTitle = "RouteName1: Pathway1" }
            };

            TlevelLoader.GetTlevelsByStatusIdAsync(Arg.Any<long>(), Arg.Any<int>())
                .Returns(mockresult);
        }

        [Fact]
        public void Then_Called_Expected_Methods()
        {
            TlevelLoader.Received().GetTlevelsByStatusIdAsync(Arg.Any<long>(), (int)TlevelReviewStatus.AwaitingConfirmation);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
