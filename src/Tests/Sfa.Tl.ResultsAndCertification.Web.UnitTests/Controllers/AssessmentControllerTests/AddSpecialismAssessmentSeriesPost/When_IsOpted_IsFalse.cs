using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.AddSpecialismAssessmentSeriesPost
{
    public class When_IsOpted_IsFalse : TestSetup
    {
        private AddSpecialismAssessmentEntryViewModel _mockresult = null;

        public override void Given()
        {
            ViewModel = new AddSpecialismAssessmentEntryViewModel
            {
                ProfileId = 1,
                AssessmentSeriesId = 11,
                AssessmentSeriesName = "Summer 2021",
                IsOpted = false
            };

            _mockresult = new AddSpecialismAssessmentEntryViewModel
            {
                ProfileId = 1,
                AssessmentSeriesId = 11,
                AssessmentSeriesName = "Summer 2021",
            };

            AssessmentLoader.GetAddAssessmentEntryAsync<AddSpecialismAssessmentEntryViewModel>(AoUkprn, ProfileId, ComponentType.Specialism).Returns(_mockresult);
        }

        [Fact]
        public void Then_Redirected_To_AssessmentDetails()
        {
            var route = (Result as RedirectToRouteResult);
            route.RouteName.Should().Be(RouteConstants.AssessmentDetails);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
        }
    }
}
