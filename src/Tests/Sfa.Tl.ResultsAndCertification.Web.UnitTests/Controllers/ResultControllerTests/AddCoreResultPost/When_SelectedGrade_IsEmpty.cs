using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.AddCoreResultPost
{
    public class When_SelectedGrade_IsEmpty : TestSetup
    {
        private int _profileId = 1;

        public override void Given()
        {
            ViewModel = new ViewModel.Result.Manual.AddCoreResultViewModel
            {
                SelectedGradeCode = string.Empty,
                ProfileId = _profileId
            };
        }

        [Fact]
        public void Then_Redirected_To_ResultDetails()
        {
            Result.Should().NotBeNull();
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.ResultDetails);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
        }
    }
}
