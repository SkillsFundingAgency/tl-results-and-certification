using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.Index
{
    public class Then_Redirected_To_SelectToReview_Action : When_Index_Action_Called
    {
        public override void Given()
        {
            var mockresult = new List<YourTlevelViewModel>
            {
                    new YourTlevelViewModel { PathwayId = 1, TlevelTitle = "RouteName1: Pathway1" },
                    new YourTlevelViewModel { PathwayId = 2, TlevelTitle = "RouteName2: Pathway2"}
            };
            TlevelLoader.GetTlevelsByStatusIdAsync(Arg.Any<long>(), Arg.Any<int>())
                .Returns(mockresult);
        }

        [Fact]
        public void Then_GetTlevelsByStatusIdAsync_Is_Called()
        {
            TlevelLoader.Received().GetTlevelsByStatusIdAsync(Arg.Any<long>(), (int)TlevelReviewStatus.AwaitingConfirmation);
        }

        [Fact]
        public void Then_GetTlevelsByStatusIdAsync_ViewModel_Return_Zero_Rows()
        {
            var actualRouteName = (Result.Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.SelectTlevel);
        }
    }
}
