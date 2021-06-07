using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaConfirmation
{
    public class When_Cache_Found : TestSetup
    {
        public override void Given()
        {
            ConfirmationCacheKey = string.Concat(CacheKey, Constants.RequestSoaConfirmation);
            SoaConfirmationViewModel = new SoaConfirmationViewModel { Uln = 1234567890, Name = "Test User" };
            CacheService.GetAndRemoveAsync<SoaConfirmationViewModel>(ConfirmationCacheKey).Returns(SoaConfirmationViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAndRemoveAsync<SoaConfirmationViewModel>(ConfirmationCacheKey);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as SoaConfirmationViewModel;

            model.Should().NotBeNull();
            model.Uln.Should().Be(SoaConfirmationViewModel.Uln);
            model.Name.Should().Be(SoaConfirmationViewModel.Name);
        }
    }
}
