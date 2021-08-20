using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ReviewTlevels
{
    public class When_Awaiting_Tlevels_Are_One : TestSetup
    {
        private List<YourTlevelViewModel> _mockResult;

        public override void Given()
        {
            _mockResult = new List<YourTlevelViewModel>
            {
                new YourTlevelViewModel { PathwayId = 1, TlevelTitle = "RouteName1: Pathway1" }
            };

            TlevelLoader.GetTlevelsByStatusIdAsync(Arg.Any<long>(), Arg.Any<int>()).Returns(_mockResult);
        }

        [Fact]
        public void Then_Called_Expected_Methods()
        {
            TlevelLoader.Received().GetTlevelsByStatusIdAsync(Arg.Any<long>(), (int)TlevelReviewStatus.AwaitingConfirmation);
        }

        [Fact]
        public void Then_Redirected_To_ReviewTlevelDetails()
        {
            var actualRoute = Result as RedirectToRouteResult;
            actualRoute.RouteName.Should().Be(RouteConstants.ReviewTlevelDetails);
            actualRoute.RouteValues.Count.Should().Be(1);
            actualRoute.RouteValues["id"].Should().Be(_mockResult[0].PathwayId);
        }
    }
}
