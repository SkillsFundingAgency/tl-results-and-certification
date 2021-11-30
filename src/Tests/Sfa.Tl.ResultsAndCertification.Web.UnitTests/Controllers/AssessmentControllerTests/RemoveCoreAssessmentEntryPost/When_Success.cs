using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;
using RemoveCoreAssessmentEntryContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment.RemoveCoreAssessmentEntry;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.RemoveCoreAssessmentEntryPost
{
    public class When_Success : TestSetup
    {
        private bool _response;
        private string _expectedSuccessBannerMsg;
        private AssessmentEntryDetailsViewModel _mockresult = null;     

        public override void Given()
        {
            ViewModel = new AssessmentEntryDetailsViewModel
            {
                ProfileId = 1,
                Uln = 12345678,
                AssessmentId = 5,
                ComponentType = ComponentType.Core,
                CanRemoveAssessmentEntry = true,
            };

            _mockresult = new AssessmentEntryDetailsViewModel
            {
                ProfileId = 1,
                AssessmentId = 5,
                AssessmentSeriesName = "summer 2021",
                PathwayDisplayName = "Test pathway (1234566789)"
            };

            _response = true;
            _expectedSuccessBannerMsg = string.Format(RemoveCoreAssessmentEntryContent.Banner_Message, _mockresult.PathwayDisplayName, _mockresult.AssessmentSeriesName);

            AssessmentLoader.GetActiveAssessmentEntryDetailsAsync(AoUkprn, ViewModel.AssessmentId, ComponentType.Core).Returns(_mockresult);
            AssessmentLoader.RemoveAssessmentEntryAsync(AoUkprn, ViewModel).Returns(_response);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AssessmentLoader.Received(1).RemoveAssessmentEntryAsync(AoUkprn, ViewModel);
            AssessmentLoader.Received(1).GetActiveAssessmentEntryDetailsAsync(AoUkprn, ViewModel.AssessmentId, ComponentType.Core);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<NotificationBannerModel>(x => x.Message.Equals(_expectedSuccessBannerMsg)), CacheExpiryTime.XSmall);
        }
                
       [Fact]
        public void Then_Redirected_To_AssessmentDetails()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.AssessmentDetails);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
        }
    }
}
