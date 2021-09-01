using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.VerifyAsync
{
    public class When_Reviewing_LastTlevel : TestSetup
    {
        private ConfirmTlevelViewModel expectedModel;
        private IEnumerable<YourTlevelViewModel> pendingReviewTlevels;

        public override void Given()
        {
            pathwayId = 10;
            isBack = true; 

            expectedModel = new ConfirmTlevelViewModel { PathwayStatusId = (int)TlevelReviewStatus.AwaitingConfirmation };

            TlevelLoader.GetVerifyTlevelDetailsByPathwayIdAsync(AoUkprn, pathwayId)
                .Returns(expectedModel);

            pendingReviewTlevels = new List<YourTlevelViewModel>
            {
                new YourTlevelViewModel { PathwayId = 1, TlevelTitle = "T1" },
            };

            TlevelLoader.GetTlevelsByStatusIdAsync(AoUkprn, (int)TlevelReviewStatus.AwaitingConfirmation).
                Returns(pendingReviewTlevels);
        }

        [Fact]
        public void Then_BackLink_Is_TlevelDashboard()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as ConfirmTlevelViewModel;

            model.Should().NotBeNull();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.TlevelsDashboard);
            model.BackLink.RouteAttributes.Should().BeEmpty();
        }
    }
}
