using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.RemoveSpecialismAssessmentEntryPost
{
    public class When_Failed : TestSetup
    {
        private RemoveSpecialismAssessmentEntryViewModel _mockresult = null;

        public override void Given()
        {
            ViewModel = new RemoveSpecialismAssessmentEntryViewModel
            {
                ProfileId = ProfileId,
                SpecialismLarId = "1|2",
                CanRemoveAssessmentEntry = true
            };

            _mockresult = new RemoveSpecialismAssessmentEntryViewModel
            {
                ProfileId = 1,
                SpecialismLarId = "1|2"
            };

            AssessmentLoader.GetRemoveSpecialismAssessmentEntriesAsync(AoUkprn, ViewModel.ProfileId, ViewModel.SpecialismLarId).Returns(_mockresult);
            AssessmentLoader.RemoveSpecialismAssessmentEntryAsync(AoUkprn, ViewModel).Returns(false);
        }

        [Fact]
        public void Then_Redirected_To_Error()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            var routeValue = (Result as RedirectToRouteResult).RouteValues["StatusCode"];
            routeName.Should().Be(RouteConstants.Error);
            routeValue.Should().Be(500);
        }
    }
}
