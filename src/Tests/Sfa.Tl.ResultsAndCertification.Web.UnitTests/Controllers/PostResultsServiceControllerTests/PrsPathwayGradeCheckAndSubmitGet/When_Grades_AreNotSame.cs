using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsPathwayGradeCheckAndSubmitGet
{
    public class When_Grades_AreNotSame : TestSetup
    {
        private PrsPathwayGradeCheckAndSubmitViewModel _mockCache = null;

        public override void Given()
        {
            var previousGrade = "B";
            var newGrade = "A";

            _mockCache = new PrsPathwayGradeCheckAndSubmitViewModel
            {
                NewGrade = newGrade,
                OldGrade = previousGrade,
                IsGradeChanged = true,
            };
            CacheService.GetAsync<PrsPathwayGradeCheckAndSubmitViewModel>(CacheKey).Returns(_mockCache);
        }

        [Fact]
        public void Then_BackLink_Route_SetTo_PrsAppealUpdatePathwayGrade()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as PrsPathwayGradeCheckAndSubmitViewModel;

            model.Should().NotBeNull();
            model.IsGradeChanged.Should().BeTrue();

            // Backlink
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsAppealUpdatePathwayGrade);
            model.BackLink.RouteAttributes.Count.Should().Be(4);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeProfileId);
            routeProfileId.Should().Be(_mockCache.ProfileId.ToString());
            model.BackLink.RouteAttributes.TryGetValue(Constants.AssessmentId, out string routeAssessmentId);
            routeAssessmentId.Should().Be(_mockCache.AssessmentId.ToString());
            model.BackLink.RouteAttributes.TryGetValue(Constants.ResultId, out string routeResultId);
            routeResultId.Should().Be(_mockCache.ResultId.ToString());
            model.BackLink.RouteAttributes.TryGetValue(Constants.IsBack, out string routeIsBack);
            routeIsBack.Should().Be(true.ToString());
        }
    }
}
