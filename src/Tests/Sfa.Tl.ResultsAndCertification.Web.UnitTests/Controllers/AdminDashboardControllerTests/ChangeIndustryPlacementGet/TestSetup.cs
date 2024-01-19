using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeIndustryPlacementGet
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {
        protected const int RegistrationPathwayId = 999;
        protected IActionResult Result { get; private set; }

        protected static AdminChangeIpViewModel ViewModel
            => new()
            {
                AdminIpCompletion = new AdminIpCompletionViewModel
                {
                    RegistrationPathwayId = RegistrationPathwayId,
                    LearnerName = "Kevin Smith",
                    Uln = 1234567890,
                    Provider = "Barnsley College (10000536)",
                    TlevelName = "Education and Early Years",
                    AcademicYear = 2020,
                    StartYear = "2021 to 2022",
                    IndustryPlacementStatus = IndustryPlacementStatus.Completed
                }
            };

        public async override Task When()
        {
            Result = await Controller.AdminChangeIndustryPlacementAsync(RegistrationPathwayId);
        }

        protected void AssertViewResult()
        {
            var model = Result.ShouldBeViewResult<AdminIpCompletionViewModel>();
            var ipCompletionModel = ViewModel.AdminIpCompletion;

            model.RegistrationPathwayId.Should().Be(ipCompletionModel.RegistrationPathwayId);
            model.Uln.Should().Be(ipCompletionModel.Uln);
            model.LearnerName.Should().Be(ipCompletionModel.LearnerName);
            model.Provider.Should().Be(ipCompletionModel.Provider);
            model.TlevelName.Should().Be(ipCompletionModel.TlevelName);
            model.AcademicYear.Should().Be(ipCompletionModel.AcademicYear);
            model.StartYear.Should().Be(ipCompletionModel.StartYear);
            model.IndustryPlacementStatus.Should().Be(ipCompletionModel.IndustryPlacementStatus);

            // Learner
            model.SummaryLearner.Title.Should().Be(AdminChangeIndustryPlacement.Title_Learner_Text);
            model.SummaryLearner.Value.Should().Be(ipCompletionModel.LearnerName);

            //Uln
            model.SummaryULN.Title.Should().Be(AdminChangeIndustryPlacement.Title_ULN_Text);
            model.SummaryULN.Value.Should().Be(ipCompletionModel.Uln.ToString());

            // Provider
            model.SummaryProvider.Title.Should().Be(AdminChangeIndustryPlacement.Title_Provider_Text);
            model.SummaryProvider.Value.Should().Be(ipCompletionModel.Provider);

            // TLevelTitle
            model.SummaryTlevel.Title.Should().Be(AdminChangeIndustryPlacement.Title_TLevel_Text);
            model.SummaryTlevel.Value.Should().Be(ipCompletionModel.TlevelName);

            // Start Year
            model.SummaryAcademicYear.Title.Should().Be(AdminChangeIndustryPlacement.Title_StartYear_Text);
            model.SummaryAcademicYear.Value.Should().Be(ipCompletionModel.StartYear);

            // Industry placement
            model.SummaryIndustryPlacementStatus.Title.Should().Be(AdminChangeIndustryPlacement.Title_Industry_Placement_Status);
            model.SummaryIndustryPlacementStatus.Value.Should().Be(ipCompletionModel.GetIndustryPlacementDisplayText(ipCompletionModel.IndustryPlacementStatus));

            // Back link
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AdminLearnerRecord);
        }
    }
}