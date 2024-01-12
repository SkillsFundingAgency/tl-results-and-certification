using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeIndustryPlacementGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        protected AdminIpCompletionViewModel Mockresult = null;

        public override void Given()
        {
            RegistrationPathwayId = 10;
            AdminChangeIndustryPlacementViewModel = new() { IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.Completed };
            Mockresult = new AdminIpCompletionViewModel
            {
                RegistrationPathwayId = 10,
                Uln = 1235469874,
                LearnerName = "firstname lastname",
                Provider = "provider-name (58794528)",
                TlevelName = "t-level-name",
                AcademicYear = 2020,
                StartYear = "2021 to 2022",
                IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.Completed
            };

            AdminDashboardLoader.GetAdminLearnerRecordAsync<AdminIpCompletionViewModel>(RegistrationPathwayId).Returns(Mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.Received(1).GetAdminLearnerRecordAsync<AdminIpCompletionViewModel>(RegistrationPathwayId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as AdminIpCompletionViewModel;

            model.RegistrationPathwayId.Should().Be(Mockresult.RegistrationPathwayId);
            model.Uln.Should().Be(Mockresult.Uln);
            model.LearnerName.Should().Be(Mockresult.LearnerName);
            model.Provider.Should().Be(Mockresult.Provider);
            model.TlevelName.Should().Be(Mockresult.TlevelName);
            model.StartYear.Should().Be("2021 to 2022");
            model.IndustryPlacementStatus.Should().Be(Common.Enum.IndustryPlacementStatus.Completed);

            // Learner
            model.SummaryLearner.Title.Should().Be(AdminChangeIndustryPlacement.Title_Learner_Text);
            model.SummaryLearner.Value.Should().Be(Mockresult.LearnerName);

            //Uln
            model.SummaryULN.Title.Should().Be(AdminChangeIndustryPlacement.Title_ULN_Text);
            model.SummaryULN.Value.Should().Be(Mockresult.Uln.ToString());

            // Provider
            model.SummaryProvider.Title.Should().Be(AdminChangeIndustryPlacement.Title_Provider_Text);
            model.SummaryProvider.Value.Should().Be(Mockresult.Provider);

            // TLevelTitle
            model.SummaryTlevel.Title.Should().Be(AdminChangeIndustryPlacement.Title_TLevel_Text);
            model.SummaryTlevel.Value.Should().Be(Mockresult.TlevelName);

            // Start Year
            model.SummaryAcademicYear.Title.Should().Be(AdminChangeIndustryPlacement.Title_StartYear_Text);
            model.SummaryAcademicYear.Value.Should().Be(Mockresult.StartYear);

            // Industry placement
            model.SummaryIndustryPlacementStatus.Title.Should().Be(AdminChangeIndustryPlacement.Title_Industry_Placement_Status);
            model.SummaryIndustryPlacementStatus.Value.Should().Be(Mockresult.GetIndustryPlacementDisplayText);

            // Back link
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AdminLearnerRecord);
        }
    }
}
