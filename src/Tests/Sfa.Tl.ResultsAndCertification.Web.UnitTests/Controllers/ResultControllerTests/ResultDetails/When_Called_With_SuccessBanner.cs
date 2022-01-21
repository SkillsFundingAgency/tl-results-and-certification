using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.ResultDetails
{
    public class When_Called_With_SuccessBanner : TestSetup
    {
        private ResultDetailsViewModel _mockResult = null;
        private readonly string _expectedSuccessBannerMessage = "result has been added";

        public override void Given()
        {
            _mockResult = new ResultDetailsViewModel
            {                
                Uln = 1234567890,
                Firstname = "First",
                Lastname = "Last",
                DateofBirth = DateTime.Now.AddYears(-30),
                ProviderName = "Test Provider",
                ProviderUkprn = 1234567891,
                TlevelTitle = "Tlevel title",
            };

            var notificationBannerModel = new NotificationBannerModel { Message = _expectedSuccessBannerMessage };
            ResultLoader.GetResultDetailsAsync(AoUkprn, ProfileId).Returns(_mockResult);
            CacheService.GetAndRemoveAsync<NotificationBannerModel>(CacheKey).Returns(notificationBannerModel);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            ResultLoader.Received(1).GetResultDetailsAsync(AoUkprn, ProfileId);
            CacheService.Received(1).GetAndRemoveAsync<NotificationBannerModel>(CacheKey);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ResultDetailsViewModel));

            var model = viewResult.Model as ResultDetailsViewModel;
            model.Should().NotBeNull();

            model.SuccessBanner.Should().NotBeNull();
            model.SuccessBanner.Message.Should().Be(_expectedSuccessBannerMessage);
        }
    }
}
