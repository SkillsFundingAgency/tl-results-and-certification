using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.AddCoreAssessmentSeriesGet
{
    public class When_AssessmentSeries_Found : TestSetup
    {
        private AddAssessmentSeriesViewModel mockresult = null;

        public override void Given()
        {
            mockresult = new AddAssessmentSeriesViewModel
            {
                ProfileId = 1,
                AssessmentSeriesId = 11,
                AssessmentSeriesName = "Summer 2021",
            };

            AssessmentLoader.GetAvailableAssessmentSeriesAsync(AoUkprn, ProfileId, AssessmentEntryType.Core).Returns(mockresult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(AddAssessmentSeriesViewModel));

            var model = viewResult.Model as AddAssessmentSeriesViewModel;
            model.Should().NotBeNull();

            model.ProfileId.Should().Be(mockresult.ProfileId);
            model.AssessmentSeriesId.Should().Be(mockresult.AssessmentSeriesId);
            model.AssessmentSeriesName.Should().Be(mockresult.AssessmentSeriesName);
            model.IsOpted.Should().BeNull();

            // Backlink
            var backLink = model.BackLink;
            backLink.RouteName.Should().Be(RouteConstants.AssessmentDetails);
            backLink.RouteAttributes.Count.Should().Be(1);
            backLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeValue);
            routeValue.Should().Be(mockresult.ProfileId.ToString());
        }
    }
}
