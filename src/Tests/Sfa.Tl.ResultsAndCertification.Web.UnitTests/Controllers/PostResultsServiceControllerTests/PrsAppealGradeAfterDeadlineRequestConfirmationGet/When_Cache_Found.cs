using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealGradeAfterDeadlineRequestConfirmationGet
{
    public class When_Cache_Found : TestSetup
    {
        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 10;

            ViewModel = new PrsAppealGradeAfterDeadlineRequestConfirmationViewModel { ProfileId = ProfileId, AssessmentId = AssessmentId };
            CacheService.GetAndRemoveAsync<PrsAppealGradeAfterDeadlineRequestConfirmationViewModel>(CacheKey).Returns(ViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAndRemoveAsync<PrsAppealGradeAfterDeadlineRequestConfirmationViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as PrsAppealGradeAfterDeadlineRequestConfirmationViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(ViewModel.ProfileId);
            model.AssessmentId.Should().Be(ViewModel.AssessmentId);
        }
    }
}
