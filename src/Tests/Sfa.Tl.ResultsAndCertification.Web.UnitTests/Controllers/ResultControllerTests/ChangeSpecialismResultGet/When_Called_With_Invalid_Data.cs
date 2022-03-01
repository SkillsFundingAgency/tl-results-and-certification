using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.ChangeSpecialismResultGet
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private ManageSpecialismResultViewModel _mockResult = null;

        public override void Given()
        {
            ResultLoader.GetManageSpecialismResultAsync(AoUkprn, ProfileId, AssessmentId, false).Returns(_mockResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ResultLoader.Received(1).GetManageSpecialismResultAsync(AoUkprn, ProfileId, AssessmentId, true);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
