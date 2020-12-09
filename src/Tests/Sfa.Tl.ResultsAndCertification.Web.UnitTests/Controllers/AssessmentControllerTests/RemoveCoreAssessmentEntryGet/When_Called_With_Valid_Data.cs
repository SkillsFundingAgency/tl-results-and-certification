using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using AssessmentDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment.AssessmentDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.RemoveCoreAssessmentEntryGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private AssessmentEntryDetailsViewModel mockresult = null;

        public override void Given()
        {
            mockresult = new AssessmentEntryDetailsViewModel
            {
                ProfileId = 1,
                AssessmentId = 5,
                AssessmentSeriesName = "Summer 2021"
            };

            AssessmentLoader.GetActiveAssessmentEntryDetailsAsync(AoUkprn, ProfileId, AssessmentEntryType.Core).Returns(mockresult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(AssessmentEntryDetailsViewModel));

            var model = viewResult.Model as AssessmentEntryDetailsViewModel;
            model.Should().NotBeNull();

            model.ProfileId.Should().Be(mockresult.ProfileId);
            model.AssessmentId.Should().Be(mockresult.AssessmentId);
            model.AssessmentSeriesName.Should().Be(mockresult.AssessmentSeriesName);

            // Backlink
            var backLink = model.BackLink;
            backLink.RouteName.Should().Be(RouteConstants.AssessmentDetails);
            backLink.RouteAttributes.Count.Should().Be(1);
            backLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeValue);
            routeValue.Should().Be(mockresult.ProfileId.ToString());
        }
    }
}
