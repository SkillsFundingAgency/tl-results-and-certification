using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.VerifyAsync
{
    public class Then_Null_ViewModel_Reredirected_To_PageNotFound : When_VerifyAsync_Get_Action_Is_Called
    {
        private ConfirmTlevelViewModel viewModel;

        public override void Given()
        {
            pathwayId = 10;

            TlevelLoader.GetVerifyTlevelDetailsByPathwayIdAsync(ukprn, pathwayId)
                .Returns(viewModel);
        }

        [Fact]
        public void Then_Null_ViewModel_Redirected_To_Route_PageNotFound()
        {
            var routeName = (Result.Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }

        [Fact]
        public void Then_GetVerifyTlevelDetailsByPathwayIdAsync_Method_Is_Called()
        {
            TlevelLoader.Received().GetVerifyTlevelDetailsByPathwayIdAsync(ukprn, pathwayId);
        }
    }
}
