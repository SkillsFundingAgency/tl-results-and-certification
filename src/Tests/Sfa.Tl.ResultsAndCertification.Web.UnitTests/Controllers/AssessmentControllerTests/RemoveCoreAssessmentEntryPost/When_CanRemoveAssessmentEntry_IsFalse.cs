using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.RemoveCoreAssessmentEntryPost
{
    public class When_CanRemoveAssessmentEntry_IsFalse : TestSetup
    {
        private AssessmentEntryDetailsViewModel _mockresult = null;

        public override void Given()
        {
            ViewModel = new AssessmentEntryDetailsViewModel
            {
                ProfileId = ProfileId,
                AssessmentId = 5,
                AssessmentSeriesName = "Summer 2021",
                CanRemoveAssessmentEntry = false
            };

            _mockresult = new AssessmentEntryDetailsViewModel
            {
                ProfileId = 1,
                AssessmentId = 5,
                AssessmentSeriesName = "Summer 2021"
            };

            AssessmentLoader.GetActiveAssessmentEntryDetailsAsync(AoUkprn, ViewModel.AssessmentId, ComponentType.Core).Returns(_mockresult);
        }

        [Fact]
        public void Then_Redirected_To_AssessmentDetails()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.AssessmentDetails);
            route.RouteValues[Constants.ProfileId].Should().Be(ProfileId);
        }
    }
}
