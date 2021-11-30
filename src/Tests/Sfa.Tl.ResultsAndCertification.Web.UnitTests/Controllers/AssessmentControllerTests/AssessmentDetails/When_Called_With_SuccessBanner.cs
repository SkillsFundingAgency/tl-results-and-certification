using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.AssessmentDetails
{
    public class When_Called_With_SuccessBanner : TestSetup
    {
        private AssessmentDetailsViewModel _mockresult = null;
        private readonly string _expectedSuccessBannerMessage = "Add or remove entry is Success";

        public override void Given()
        {
            _mockresult = new AssessmentDetailsViewModel
            {
                ProfileId = 1,
                Uln = 1234567890,
                Firstname = "First",
                Lastname = "Last",
                DateofBirth = System.DateTime.UtcNow.AddYears(-30),
                ProviderName = "Test Provider",
                ProviderUkprn = 1234567,
                TlevelTitle = "Tlevel Title",
                PathwayStatus = RegistrationPathwayStatus.Active
            };

            var notificationBannerModel = new NotificationBannerModel { Message = _expectedSuccessBannerMessage };
            AssessmentLoader.GetAssessmentDetailsAsync<AssessmentDetailsViewModel>(AoUkprn, ProfileId, RegistrationPathwayStatus.Active).Returns(_mockresult);
            CacheService.GetAndRemoveAsync<NotificationBannerModel>(CacheKey).Returns(notificationBannerModel);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            AssessmentLoader.Received(1).GetAssessmentDetailsAsync<AssessmentDetailsViewModel>(AoUkprn, ProfileId, RegistrationPathwayStatus.Active);
            CacheService.Received(1).GetAndRemoveAsync<NotificationBannerModel>(CacheKey);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(AssessmentDetailsViewModel));

            var model = viewResult.Model as AssessmentDetailsViewModel;
            model.Should().NotBeNull();

            model.SuccessBanner.Should().NotBeNull();
            model.SuccessBanner.Message.Should().Be(_expectedSuccessBannerMessage);
        }
    }
}
