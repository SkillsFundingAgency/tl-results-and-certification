using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsGradeChangeRequestConfirmationGet
{
    public class When_Cache_Found : TestSetup
    {
        public override void Given()
        {
            ProfileId = 1;
                
            ViewModel = new PrsGradeChangeRequestConfirmationViewModel { ProfileId = ProfileId };
            CacheService.GetAndRemoveAsync<PrsGradeChangeRequestConfirmationViewModel>(CacheKey).Returns(ViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAndRemoveAsync<PrsGradeChangeRequestConfirmationViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as PrsGradeChangeRequestConfirmationViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(ViewModel.ProfileId);
            model.NavigationOption.Should().BeNull();
        }
    }
}
