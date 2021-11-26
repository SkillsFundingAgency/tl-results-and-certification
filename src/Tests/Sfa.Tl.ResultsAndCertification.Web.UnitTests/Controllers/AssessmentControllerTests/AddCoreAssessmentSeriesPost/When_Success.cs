using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;
using AddCoreAssessmentEntryContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment.AddCoreAssessmentEntry;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.AddCoreAssessmentSeriesPost
{
    public class When_Success : TestSetup
    {
        private AddAssessmentEntryResponse _addAssessmentEntryResponse;
        private AddAssessmentEntryViewModel _mockresult = null;
        private string _expectedSuccessBannerMsg;

        public override void Given()
        {
            ViewModel = new AddAssessmentEntryViewModel
            {
                ProfileId = 1,
                AssessmentSeriesId = 11,
                AssessmentSeriesName = "Summer 2021",
                IsOpted = true,
                PathwayDisplayName = "Childcare"
            };

            _addAssessmentEntryResponse = new AddAssessmentEntryResponse 
            {
                IsSuccess = true,
                Uln = 1234567890
            };

            _mockresult = new AddAssessmentEntryViewModel
            {
                ProfileId = 1,
                AssessmentSeriesId = 11,
                AssessmentSeriesName = "Summer 2021",
                PathwayDisplayName = "Childcare (1234566789)"
            };

            _expectedSuccessBannerMsg = string.Format(AddCoreAssessmentEntryContent.Banner_Message, _mockresult.AssessmentSeriesName, _mockresult.PathwayDisplayName);

            AssessmentLoader.AddAssessmentEntryAsync(AoUkprn, ViewModel).Returns(_addAssessmentEntryResponse);
            AssessmentLoader.GetAddAssessmentEntryAsync(AoUkprn, ProfileId, ComponentType.Core).Returns(_mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AssessmentLoader.Received(1).GetAddAssessmentEntryAsync(AoUkprn, ProfileId, ComponentType.Core);
            AssessmentLoader.Received(1).AddAssessmentEntryAsync(AoUkprn, ViewModel);
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
