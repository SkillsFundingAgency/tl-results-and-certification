using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.IndustryPlacementUpdatedConfirmation
{
    public class When_Cache_Found : TestSetup
    {
        public override void Given()
        {
            ConfirmationCacheKey = string.Concat(CacheKey, Constants.IndustryPlacementUpdatedConfirmation);
            ViewModel = new UpdateLearnerRecordResponseViewModel { ProfileId = 10, Uln = 1234567890, Name = "Test User" };
            CacheService.GetAndRemoveAsync<UpdateLearnerRecordResponseViewModel>(ConfirmationCacheKey).Returns(ViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAndRemoveAsync<UpdateLearnerRecordResponseViewModel>(ConfirmationCacheKey);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as UpdateLearnerRecordResponseViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(ViewModel.ProfileId);
            model.Uln.Should().Be(ViewModel.Uln);
            model.Name.Should().Be(ViewModel.Name);
        }
    }
}
