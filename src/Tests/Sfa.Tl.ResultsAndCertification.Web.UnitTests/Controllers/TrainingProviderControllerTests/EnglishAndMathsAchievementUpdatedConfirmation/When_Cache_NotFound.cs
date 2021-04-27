using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.EnglishAndMathsAchievementUpdatedConfirmation
{
    public class When_Cache_NotFound : TestSetup
    {
        public override void Given()
        {
            ConfirmationCacheKey = string.Concat(CacheKey, Constants.EnglishAndMathsAchievementUpdatedConfirmation);
            CacheService.GetAndRemoveAsync<UpdateLearnerRecordResponseViewModel>(ConfirmationCacheKey).Returns(ViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAndRemoveAsync<UpdateLearnerRecordResponseViewModel>(ConfirmationCacheKey);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
