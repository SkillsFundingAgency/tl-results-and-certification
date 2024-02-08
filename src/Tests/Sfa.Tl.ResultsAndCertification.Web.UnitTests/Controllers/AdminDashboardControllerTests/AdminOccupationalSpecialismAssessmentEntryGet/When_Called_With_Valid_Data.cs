using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminOccupationalSpecialismAssessmentEntry
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        protected AdminOccupationalSpecialismViewModel Mockresult = null;

        public override void Given()
      {
            RegistrationPathwayId = 10;
            Mockresult = new AdminOccupationalSpecialismViewModel
            {
                Uln = 1235469874,
                LearnerName = "firstname lastname",
                Provider = "provider-name",
                TlevelName = "t-level-name",
                StartYear = 2020,
                DisplayStartYear = "2021 to 2022",
                PathwayDisplayName = "pathway name"
            };

            AdminDashboardLoader.GetAdminLearnerRecordWithOccupationalSpecialism(RegistrationPathwayId, SpecialismId).Returns(Mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.Received(1).GetAdminLearnerRecordWithOccupationalSpecialism(RegistrationPathwayId, SpecialismId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as AdminOccupationalSpecialismViewModel;

            model.RegistrationPathwayId.Should().Be(Mockresult.RegistrationPathwayId);
            model.Uln.Should().Be(Mockresult.Uln);
            model.LearnerName.Should().Be(Mockresult.LearnerName);
            model.Provider.Should().Be(Mockresult.Provider);
            model.TlevelName.Should().Be(Mockresult.TlevelName);
            model.StartYear.Should().Be(Mockresult.StartYear);
            model.DisplayStartYear.Should().Be("2021 to 2022");
            model.PathwayDisplayName.Should().Be(Mockresult.PathwayDisplayName);

            // Learner
            model.SummaryLearner.Title.Should().Be(AdminLearnerAssessmentEntry.Title_Learner_Text);
            model.SummaryLearner.Value.Should().Be(Mockresult.LearnerName);

            //Uln
            model.SummaryULN.Title.Should().Be(AdminLearnerAssessmentEntry.Title_ULN_Text);
            model.SummaryULN.Value.Should().Be(Mockresult.Uln.ToString());

            // Provider
            model.SummaryProvider.Title.Should().Be(AdminLearnerAssessmentEntry.Title_Provider_Text);
            model.SummaryProvider.Value.Should().Be(Mockresult.Provider);

            // TLevelTitle
            model.SummaryTlevel.Title.Should().Be(AdminLearnerAssessmentEntry.Title_TLevel_Text);
            model.SummaryTlevel.Value.Should().Be(Mockresult.TlevelName);

            // Display Start Year
            model.SummaryAcademicYear.Title.Should().Be(AdminLearnerAssessmentEntry.Title_StartYear_Text);
            model.SummaryAcademicYear.Value.Should().Be(Mockresult.DisplayStartYear);

            // Back link
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AdminLearnerRecord);
        }
    }
}
