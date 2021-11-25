using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.RemoveCoreAssessmentEntryPost
{
    public class When_Failed : TestSetup
    {
        private AssessmentEntryDetailsViewModel _mockresult = null;

        public override void Given()
        {
            ViewModel = new AssessmentEntryDetailsViewModel
            {
                ProfileId = ProfileId,
                AssessmentId = 5,
                ComponentType = Common.Enum.ComponentType.Core,
                CanRemoveAssessmentEntry = true
            };

            _mockresult = new AssessmentEntryDetailsViewModel
            {
                ProfileId = 1,
                AssessmentId = 5,
                AssessmentSeriesName = "Summer 2021"
            };

            AssessmentLoader.GetActiveAssessmentEntryDetailsAsync(AoUkprn, ViewModel.AssessmentId, ComponentType.Core).Returns(_mockresult);
            AssessmentLoader.RemoveAssessmentEntryAsync(AoUkprn, ViewModel).Returns(false);
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
