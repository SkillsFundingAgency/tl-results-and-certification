using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminCoreComponentAssessmentPost;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangeCoreAssessmentPost
{
    public class When_Failed : TestSetup
    {
        public override void Given()
        {
            AdminReviewChangesCoreAssessmentViewModel = new AdminReviewChangesCoreAssessmentViewModel()
            {
                RegistrationPathwayId = 1,
                ContactName = "firstname",
                AdminCoreComponentViewModel = new AdminCoreComponentViewModel()
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

            AdminDashboardLoader.ProcessAddCoreAssessmentRequestAsync(AdminReviewChangesCoreAssessmentViewModel).Returns(false);
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