using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangeSpecialismAssessmentGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        protected AdminReviewChangesSpecialismAssessmentViewModel Mockresult = null;

        public override void Given()
        {
            RegistrationPathwayId = 10;

            AdminOccupationalSpecialismViewModel AdminOccupationalSpecialismViewModel = new()
            {
                Uln = 1235469874,
                LearnerName = "firstname",
                Provider = "provider-name",
                TlevelName = "t-level-name",
                
            };

            

            Mockresult = new AdminReviewChangesSpecialismAssessmentViewModel
            {
                ContactName = "contact-name",
                Day = "01",
                Month = "01",
                Year = "1970",
                ChangeReason = "change-reason",
                ZendeskId = "1234567890",
                AdminOccupationalSpecialismViewModel = AdminOccupationalSpecialismViewModel
            };

            AdminDashboardLoader.GetAdminLearnerRecordAsync<AdminReviewChangesSpecialismAssessmentViewModel>(RegistrationPathwayId).Returns(Mockresult);
            CacheService.GetAsync<AdminOccupationalSpecialismViewModel>(CacheKey).Returns(AdminOccupationalSpecialismViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as AdminReviewChangesSpecialismAssessmentViewModel;

            model.AdminOccupationalSpecialismViewModel.RegistrationPathwayId.Should().Be(Mockresult.AdminOccupationalSpecialismViewModel.RegistrationPathwayId);
            model.AdminOccupationalSpecialismViewModel.Uln.Should().Be(Mockresult.AdminOccupationalSpecialismViewModel.Uln);
            model.AdminOccupationalSpecialismViewModel.Provider.Should().Be(Mockresult.AdminOccupationalSpecialismViewModel.Provider);
            model.AdminOccupationalSpecialismViewModel.TlevelName.Should().Be(Mockresult.AdminOccupationalSpecialismViewModel.TlevelName);

            // Learner
            model.AdminOccupationalSpecialismViewModel.SummaryLearner.Title.Should().Be(ReviewChangeAssessment.Title_Learner_Text);
            model.AdminOccupationalSpecialismViewModel.SummaryLearner.Value.Should().Be(Mockresult.AdminOccupationalSpecialismViewModel.LearnerName);

            //Uln
            model.AdminOccupationalSpecialismViewModel.SummaryULN.Title.Should().Be(ReviewChangeAssessment.Title_ULN_Text);
            model.AdminOccupationalSpecialismViewModel.SummaryULN.Value.Should().Be(Mockresult.AdminOccupationalSpecialismViewModel.Uln.ToString());

            // Provider
            model.AdminOccupationalSpecialismViewModel.SummaryProvider.Title.Should().Be(ReviewChangeAssessment.Title_Provider_Text);
            model.AdminOccupationalSpecialismViewModel.SummaryProvider.Value.Should().Be(Mockresult.AdminOccupationalSpecialismViewModel.Provider);

            // TLevelTitle
            model.AdminOccupationalSpecialismViewModel.SummaryTlevel.Title.Should().Be(ReviewChangeAssessment.Title_TLevel_Text);
            model.AdminOccupationalSpecialismViewModel.SummaryTlevel.Value.Should().Be(Mockresult.AdminOccupationalSpecialismViewModel.TlevelName);

            // Back link
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AdminOccupationalSpecialisAssessmentEntry);
        }
    }
}
