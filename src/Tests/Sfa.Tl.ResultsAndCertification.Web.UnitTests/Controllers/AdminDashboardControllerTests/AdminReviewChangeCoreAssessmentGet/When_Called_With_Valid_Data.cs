using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangeCoreAssessmentGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        protected AdminReviewChangesCoreAssessmentViewModel Mockresult = null;

        public override void Given()
        {
            RegistrationPathwayId = 10;

            AdminCoreComponentViewModel adminCoreComponentViewModel = new()
            {
                Uln = 1235469874,
                LearnerName = "firstname",
                Provider = "provider-name",
                TlevelName = "t-level-name",
                
            };

            

            Mockresult = new AdminReviewChangesCoreAssessmentViewModel
            {
                ContactName = "contact-name",
                Day = "01",
                Month = "01",
                Year = "1970",
                ChangeReason = "change-reason",
                ZendeskId = "1234567890",
                AdminCoreComponentViewModel = adminCoreComponentViewModel
            };

            AdminDashboardLoader.GetAdminLearnerRecordAsync<AdminReviewChangesCoreAssessmentViewModel>(RegistrationPathwayId).Returns(Mockresult);
            CacheService.GetAsync<AdminCoreComponentViewModel>(CacheKey).Returns(adminCoreComponentViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as AdminReviewChangesCoreAssessmentViewModel;

            model.AdminCoreComponentViewModel.RegistrationPathwayId.Should().Be(Mockresult.AdminCoreComponentViewModel.RegistrationPathwayId);
            model.AdminCoreComponentViewModel.Uln.Should().Be(Mockresult.AdminCoreComponentViewModel.Uln);
            model.AdminCoreComponentViewModel.Provider.Should().Be(Mockresult.AdminCoreComponentViewModel.Provider);
            model.AdminCoreComponentViewModel.TlevelName.Should().Be(Mockresult.AdminCoreComponentViewModel.TlevelName);

            // Learner
            model.AdminCoreComponentViewModel.SummaryLearner.Title.Should().Be(ReviewChangeAssessment.Title_Learner_Text);
            model.AdminCoreComponentViewModel.SummaryLearner.Value.Should().Be(Mockresult.AdminCoreComponentViewModel.LearnerName);

            //Uln
            model.AdminCoreComponentViewModel.SummaryULN.Title.Should().Be(ReviewChangeAssessment.Title_ULN_Text);
            model.AdminCoreComponentViewModel.SummaryULN.Value.Should().Be(Mockresult.AdminCoreComponentViewModel.Uln.ToString());

            // Provider
            model.AdminCoreComponentViewModel.SummaryProvider.Title.Should().Be(ReviewChangeAssessment.Title_Provider_Text);
            model.AdminCoreComponentViewModel.SummaryProvider.Value.Should().Be(Mockresult.AdminCoreComponentViewModel.Provider);

            // TLevelTitle
            model.AdminCoreComponentViewModel.SummaryTlevel.Title.Should().Be(ReviewChangeAssessment.Title_TLevel_Text);
            model.AdminCoreComponentViewModel.SummaryTlevel.Value.Should().Be(Mockresult.AdminCoreComponentViewModel.TlevelName);

            // Back link
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AdminCoreComponentAssessmentEntry);
        }
    }
}
