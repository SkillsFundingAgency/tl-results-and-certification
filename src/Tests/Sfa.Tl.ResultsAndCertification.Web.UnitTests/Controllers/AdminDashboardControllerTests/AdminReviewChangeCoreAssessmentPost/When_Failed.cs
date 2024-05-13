using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminCoreComponentAssessmentPost;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangeCoreAssessmentPost
{
    public class When_Failed : TestSetup
    {
        private AdminReviewChangesCoreAssessmentViewModel MockResult = null;
        private NotificationBannerModel _expectedNotificationBannerModel;
        public override void Given()
        {
            var isSuccess = false;
            AdminReviewChangesCoreAssessmentViewModel = new AdminReviewChangesCoreAssessmentViewModel()
            {
                RegistrationPathwayId = 1,
                ContactName = "firstname",   
                AdminCoreComponentViewModel =  new AdminCoreComponentViewModel()
                {
                    Uln = 1100000001,
                    Provider = "provider-name (10000536)",
                    TlevelName = "t-level-name",
                    StartYear = 2022,
                    DisplayStartYear = "2022 to 2023",
                },
               
                ChangeReason = "change-reason",
                Day = "01",
                Month = "01",
                Year = "1970"
            };


            AdminDashboardLoader.ProcessAddCoreAssessmentRequestAsync(AdminReviewChangesCoreAssessmentViewModel).Returns(isSuccess = false);

        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.Received(1).ProcessAddCoreAssessmentRequestAsync(AdminReviewChangesCoreAssessmentViewModel);

        }

        [Fact]
        public void Then_Redirected_To_ProblemWithService()
        {
            var route = Result as RedirectToActionResult;
            route.ActionName.Should().Be(RouteConstants.ProblemWithService);

        }
    }
}
