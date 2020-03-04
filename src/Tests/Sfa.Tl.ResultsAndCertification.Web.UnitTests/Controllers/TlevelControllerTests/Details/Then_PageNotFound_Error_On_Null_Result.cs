using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.Details
{
    public class Then_PageNotFound_Error_On_Null_Result : When_Details_Action_Called
    {
        private TLevelDetailsViewModel mockresult;

        public override void Given()
        {
            TlevelLoader.GetTlevelDetailsByPathwayIdAsync(ukPrn, id)
                .Returns(mockresult);
        }

        [Fact]
        public void Then_PageNotFound_Is_Returned_OnNull_Result()
        {
            var actualRouteName = (Result.Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
