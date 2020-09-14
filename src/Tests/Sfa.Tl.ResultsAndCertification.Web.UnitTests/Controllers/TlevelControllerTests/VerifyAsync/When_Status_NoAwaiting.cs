using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.VerifyAsync
{
    public class When_Status_NoAwaiting : TestSetup
    {
        private ConfirmTlevelViewModel viewModel;

        public override void Given()
        {
            pathwayId = 10;
            viewModel = new ConfirmTlevelViewModel { PathwayStatusId = (int)TlevelReviewStatus.Confirmed };

            TlevelLoader.GetVerifyTlevelDetailsByPathwayIdAsync(ukprn, pathwayId)
                .Returns(viewModel);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result.Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
