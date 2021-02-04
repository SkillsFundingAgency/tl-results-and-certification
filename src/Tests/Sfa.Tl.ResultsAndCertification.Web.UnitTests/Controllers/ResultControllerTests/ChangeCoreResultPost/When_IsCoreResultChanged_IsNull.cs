using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.ChangeCoreResultPost
{
    public class When_IsCoreResultChanged_IsNull : TestSetup
    {
        private readonly bool? mockResult = null;
        public override void Given()
        {
           ResultLoader.IsCoreResultChanged(AoUkprn, Arg.Any<ManageCoreResultViewModel>()).Returns(mockResult);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
