using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.AddAssessmentEntryConfirmation
{
    public class When_TempData_Found : TestSetup
    {
        public override void Given()
        {
            ViewModel = new AddAssessmentEntryConfirmationViewModel { ProfileId = 1, UniqueLearnerReference = 1234567890 };
            CacheService.GetAndRemoveAsync<AddAssessmentEntryConfirmationViewModel>(Arg.Any<string>()).Returns(ViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as AddAssessmentEntryConfirmationViewModel;

            model.Should().NotBeNull();
            model.UniqueLearnerReference.Should().Be(ViewModel.UniqueLearnerReference);
            model.ProfileId.Should().Be(ViewModel.ProfileId);
        }
    }
}
