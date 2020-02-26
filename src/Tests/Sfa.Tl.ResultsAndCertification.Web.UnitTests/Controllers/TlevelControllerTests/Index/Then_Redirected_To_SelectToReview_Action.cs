using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
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
            var mockresult = new List<YourTlevelsViewModel>
            {
                    new YourTlevelsViewModel { PathwayId = 1, StatusId = 1, TlevelTitle = "RouteName1: Pathway1" },
                    new YourTlevelsViewModel { PathwayId = 2, StatusId = 1, TlevelTitle = "RouteName2: Pathway2"}
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
            var actualActionName = (Result.Result as RedirectToActionResult).ActionName;
            var expectedActionName = nameof(TlevelController.SelectToReview);

            actualActionName.Should().Be(expectedActionName);
        }
    }
}
