using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;
using RequestReplacementDocumentContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.RequestReplacementDocument;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.RequestReplacementDocumentPost
{
    public class When_Success : TestSetup
    {
        private NotificationBannerModel _expectedSuccessBannerMsg;

        public override void Given()
        {
            ProfileId = 1;
            _expectedSuccessBannerMsg = new NotificationBannerModel { HeaderMessage = RequestReplacementDocumentContent.Success_Header_Replacement_Document_Requested, Message = RequestReplacementDocumentContent.Success_Message_Replacement_Documents, DisplayMessageBody = true, IsRawHtml = true };

            ViewModel = new RequestReplacementDocumentViewModel { ProfileId = ProfileId, Uln = 987456123, PrintCertificateId = 1, ProviderAddress = new ViewModel.ProviderAddress.AddressViewModel { AddressId = 1 } };
            TrainingProviderLoader.CreateReplacementDocumentPrintingRequestAsync(ProviderUkprn, ViewModel).Returns(true);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            TrainingProviderLoader.Received(1).CreateReplacementDocumentPrintingRequestAsync(ProviderUkprn, ViewModel);

            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<NotificationBannerModel>(x =>
                x.HeaderMessage.Equals(_expectedSuccessBannerMsg.HeaderMessage, System.StringComparison.InvariantCultureIgnoreCase) &&
                x.Message.Equals(_expectedSuccessBannerMsg.Message, System.StringComparison.InvariantCultureIgnoreCase) &&
                x.DisplayMessageBody == true &&
                x.IsRawHtml == true),
                CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Redirected_To_LearnerRecordDetails()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.LearnerRecordDetails);
            route.RouteValues[Constants.ProfileId].Should().Be(ProfileId);
        }
    }
}
