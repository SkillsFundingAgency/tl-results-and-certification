using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System.Collections.Generic;
using Xunit;
using RemoveSpecialismAssessmentEntriesContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment.RemoveSpecialismAssessmentEntries;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.RemoveSpecialismAssessmentEntryPost
{
    public class When_Success : TestSetup
    {
        private bool _response;
        private string _expectedSuccessBannerMsg;
        private RemoveSpecialismAssessmentEntryViewModel _mockresult = null;

        public override void Given()
        {
            ViewModel = new RemoveSpecialismAssessmentEntryViewModel
            {
                ProfileId = 1,
                Uln = 12345678,
                SpecialismAssessmentIds = "1|2",
                CanRemoveAssessmentEntry = true,
            };

            _mockresult = new RemoveSpecialismAssessmentEntryViewModel
            {
                ProfileId = 1,
                SpecialismAssessmentIds = "1|2",
                AssessmentSeriesName = "summer 2022",
                SpecialismDetails = new List<SpecialismViewModel>
                {
                    new SpecialismViewModel
                    {
                        Id = 1,
                        LarId = "ZT1234567",
                        Name = "Specialism 1",
                        DisplayName = "Specialism 1 (ZT1234567)"
                    },
                    new SpecialismViewModel
                    {
                        Id = 2,
                        LarId = "ZO565745",
                        Name = "Specialism 2",
                        DisplayName = "Specialism 2 (ZO565745)"
                    },
                }
            };

            _response = true;
            _expectedSuccessBannerMsg = string.Format(RemoveSpecialismAssessmentEntriesContent.Banner_Message, _mockresult.SpecialismDisplayName, _mockresult.AssessmentSeriesName);

            AssessmentLoader.GetRemoveSpecialismAssessmentEntriesAsync(AoUkprn, ViewModel.ProfileId, _mockresult.SpecialismAssessmentIds).Returns(_mockresult);
            AssessmentLoader.RemoveSpecialismAssessmentEntryAsync(AoUkprn, _mockresult).Returns(_response);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AssessmentLoader.Received(1).RemoveSpecialismAssessmentEntryAsync(AoUkprn, _mockresult);
            AssessmentLoader.Received(1).GetRemoveSpecialismAssessmentEntriesAsync(AoUkprn, ViewModel.ProfileId, _mockresult.SpecialismAssessmentIds);
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
