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

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangeSpecialismAssessmentPost
{
    public class When_Failed : TestSetup
    {
        
        public override void Given()
        {

            AdminReviewChangesSpecialismAssessmentViewModel = new AdminReviewChangesSpecialismAssessmentViewModel()
            {
                RegistrationPathwayId = 1,
                ContactName = "firstname",
                AdminOccupationalSpecialismViewModel = new AdminOccupationalSpecialismViewModel()
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


            AdminDashboardLoader.ProcessAddSpecialismAssessmentRequestAsync(AdminReviewChangesSpecialismAssessmentViewModel).Returns(false);

        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.Received(1).ProcessAddSpecialismAssessmentRequestAsync(AdminReviewChangesSpecialismAssessmentViewModel);

        }

        [Fact]
        public void Then_Redirected_To_ProblemWithService()
        {
            var route = Result as RedirectToActionResult;
            route.ActionName.Should().Be(RouteConstants.ProblemWithService);

        }
    }
}
