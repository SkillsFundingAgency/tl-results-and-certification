using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.AddLearnerRecordConfirmation
{
    public class When_Cache_Found : TestSetup
    {
        public override void Given()
        {
            ConfirmationCacheKey = string.Concat(CacheKey, Constants.AddLearnerRecordConfirmation);
            LearnerRecordConfirmationViewModel = new LearnerRecordConfirmationViewModel { Uln = 1234567890, Name = "Test User" };
            CacheService.GetAndRemoveAsync<LearnerRecordConfirmationViewModel>(ConfirmationCacheKey).Returns(LearnerRecordConfirmationViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAndRemoveAsync<LearnerRecordConfirmationViewModel>(ConfirmationCacheKey);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as LearnerRecordConfirmationViewModel;

            model.Should().NotBeNull();
            model.Uln.Should().Be(LearnerRecordConfirmationViewModel.Uln);
            model.Name.Should().Be(LearnerRecordConfirmationViewModel.Name);
        }
    }
}