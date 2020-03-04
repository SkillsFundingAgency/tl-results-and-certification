using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ConfirmationAsync
{
    public class Then_Id_Zero_ReRouted_To_PageNotFound : When_ConfirmationAsync_Is_Called
    {
        public override void Given()
        {
            Id = 0;
        }

        [Fact]
        public void Then_On_Id_Zero_Redirected_To_PageNotFound()
        {
            var routeName = (Result.Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
