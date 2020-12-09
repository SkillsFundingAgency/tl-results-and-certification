using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.AddCoreAssessmentSeriesPost
{
    public class When_Failed : TestSetup
    {
        private AddAssessmentEntryResponse AddAssessmentEntryResponse;

        public override void Given()
        {
            ViewModel = new AddAssessmentEntryViewModel
            {
                ProfileId = 1,
                AssessmentSeriesId = 11,
                AssessmentSeriesName = "Summer 2021",
                IsOpted = true
            };

            AddAssessmentEntryResponse = new AddAssessmentEntryResponse { IsSuccess = false };
            AssessmentLoader.AddAssessmentEntryAsync(AoUkprn, ViewModel).Returns(AddAssessmentEntryResponse);
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
