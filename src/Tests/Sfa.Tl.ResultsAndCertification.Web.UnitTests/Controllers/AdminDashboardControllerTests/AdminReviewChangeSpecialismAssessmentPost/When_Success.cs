using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangeSpecialismAssessmentPost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangeSpecialismAssessmentPost
{
    public class When_Success : TestSetup
    {
        public override void Given()
        {
            AdminReviewChangesSpecialismAssessmentViewModel = new AdminReviewChangesSpecialismAssessmentViewModel()
            {
               
                ContactName = "firstname",
                AdminOccupationalSpecialismViewModel = new AdminOccupationalSpecialismViewModel()
                {
                    Uln = 1100000001,
                    Provider = "provider-name (10000536)",
                    TlevelName = "t-level-name",
                    StartYear = 2022,
                    DisplayStartYear = "2022 to 2023",
                    RegistrationPathwayId = 1
                },

                ChangeReason = "change-reason",
                Day = "01",
                Month = "01",
                Year = "1970"
            };
            AdminDashboardLoader.ProcessAddSpecialismAssessmentRequestAsync(AdminReviewChangesSpecialismAssessmentViewModel).Returns(true);

        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.Received(1).ProcessAddSpecialismAssessmentRequestAsync(AdminReviewChangesSpecialismAssessmentViewModel);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<NotificationBannerModel>(p => p.Message == ReviewChangeAssessment.Message_Notification_Success), CacheExpiryTime.XSmall);

        }

        [Fact]
        public void Then_Redirected_To_Expected_ActionResult()
        {
            var route = Result as RedirectToActionResult;
            route.ActionName.Should().Be(nameof(RouteConstants.AdminLearnerRecord));
            route.RouteValues[Constants.PathwayId].Should().Be(AdminReviewChangesSpecialismAssessmentViewModel.AdminOccupationalSpecialismViewModel.RegistrationPathwayId);

        }
    }
}
