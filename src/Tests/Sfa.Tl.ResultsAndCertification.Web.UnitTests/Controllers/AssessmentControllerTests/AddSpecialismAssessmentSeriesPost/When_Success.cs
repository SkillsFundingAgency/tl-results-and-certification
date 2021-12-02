using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System.Collections.Generic;
using Xunit;
using AddCoreAssessmentEntryContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment.AddCoreAssessmentEntry;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.AddSpecialismAssessmentSeriesPost
{
    public class When_Success : TestSetup
    {
        private AddAssessmentEntryResponse _addAssessmentEntryResponse;
        private AddSpecialismAssessmentEntryViewModel _mockresult = null;
        private string _expectedSuccessBannerMsg;

        public override void Given()
        {
            ViewModel = new AddSpecialismAssessmentEntryViewModel
            {
                ProfileId = 1,
                AssessmentSeriesId = 11,
                AssessmentSeriesName = "Summer 2022",
                IsOpted = true,
                SpecialismId = 5,
                SpecialismDetails = new List<SpecialismViewModel>
                {
                    new SpecialismViewModel
                    {
                        Id = 5,
                        LarId = "ZT2158963",
                        Name = "Specialism Name1",
                        DisplayName = "Specialism Name1 (ZT2158963)",
                        Assessments = new List<SpecialismAssessmentViewModel>()
                    }
                }
            };

            _addAssessmentEntryResponse = new AddAssessmentEntryResponse
            {
                IsSuccess = true,
                Uln = 1234567890
            };

            _mockresult = new AddSpecialismAssessmentEntryViewModel
            {
                ProfileId = 1,
                AssessmentSeriesId = 11,
                AssessmentSeriesName = "Summer 2021",
                SpecialismDetails = new List<SpecialismViewModel>
                {
                    new SpecialismViewModel
                    {
                        Id = 5,
                        LarId = "ZT2158963",
                        Name = "Specialism Name1",
                        DisplayName = "Specialism Name1 (ZT2158963)",
                        Assessments = new List<SpecialismAssessmentViewModel>()
                    }
                }
            };

            _expectedSuccessBannerMsg = string.Format(AddCoreAssessmentEntryContent.Banner_Message, _mockresult.AssessmentSeriesName, _mockresult.SpecialismDetails[0].DisplayName);

            AssessmentLoader.AddSpecialismAssessmentEntryAsync(AoUkprn, _mockresult).Returns(_addAssessmentEntryResponse);
            AssessmentLoader.GetAddAssessmentEntryAsync<AddSpecialismAssessmentEntryViewModel>(AoUkprn, ProfileId, ComponentType.Specialism).Returns(_mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AssessmentLoader.Received(1).GetAddAssessmentEntryAsync<AddSpecialismAssessmentEntryViewModel>(AoUkprn, ProfileId, ComponentType.Specialism);
            AssessmentLoader.Received(1).AddSpecialismAssessmentEntryAsync(AoUkprn, _mockresult);
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
