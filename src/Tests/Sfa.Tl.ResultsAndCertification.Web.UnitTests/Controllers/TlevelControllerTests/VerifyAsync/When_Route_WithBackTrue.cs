using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.VerifyAsync
{
    public class When_Route_WithBackTrue : TestSetup
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
                new YourTlevelViewModel { PathwayId = 2, TlevelTitle = "T2" },
            };

            TlevelLoader.GetTlevelsByStatusIdAsync(AoUkprn, (int)TlevelReviewStatus.AwaitingConfirmation).
                Returns(pendingReviewTlevels);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as ConfirmTlevelViewModel;

            model.Should().NotBeNull();
            model.IsEverythingCorrect.Should().Be(false);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.SelectTlevel);
            model.BackLink.RouteAttributes.Count().Should().Be(1);
            model.BackLink.RouteAttributes["id"].Should().Be(model.PathwayId.ToString());
        }
    }
}
