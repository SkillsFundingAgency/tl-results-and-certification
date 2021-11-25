using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.RemoveCoreAssessmentEntryPost
{
    public class When_Success : TestSetup
    {
        private bool _response;
        private AssessmentEntryDetailsViewModel _mockresult = null;

        public override void Given()
        {
            ViewModel = new AssessmentEntryDetailsViewModel
            {
                ProfileId = 1,
                Uln = 12345678,
                AssessmentId = 5,
                ComponentType = ComponentType.Core,
                CanRemoveAssessmentEntry = true
            };

            _mockresult = new AssessmentEntryDetailsViewModel
            {
                ProfileId = 1,
                AssessmentId = 5,
                AssessmentSeriesName = "Summer 2021"
            };

            _response = true;

            AssessmentLoader.GetActiveAssessmentEntryDetailsAsync(AoUkprn, ViewModel.AssessmentId, ComponentType.Core).Returns(_mockresult);
            AssessmentLoader.RemoveAssessmentEntryAsync(AoUkprn, ViewModel).Returns(_response);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AssessmentLoader.Received(1).RemoveAssessmentEntryAsync(AoUkprn, ViewModel);
        }
                
       [Fact]
        public void Then_Redirected_To_AssessmentDetails()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.AssessmentDetails);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
        }
    }
}
