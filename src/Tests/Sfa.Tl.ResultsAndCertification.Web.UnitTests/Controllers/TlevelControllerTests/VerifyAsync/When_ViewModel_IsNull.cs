using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.VerifyAsync
{
    public class When_ViewModel_IsNull : TestSetup
    {
        private ConfirmTlevelViewModel viewModel = null;

        public override void Given()
        {
            pathwayId = 10;
            
            TlevelLoader.GetVerifyTlevelDetailsByPathwayIdAsync(ukprn, pathwayId)
                .Returns(viewModel);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result.Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }

        [Fact]
        public void Then_Called_Expected_Methods()
        {
            TlevelLoader.Received().GetVerifyTlevelDetailsByPathwayIdAsync(ukprn, pathwayId);
        }
    }
}
