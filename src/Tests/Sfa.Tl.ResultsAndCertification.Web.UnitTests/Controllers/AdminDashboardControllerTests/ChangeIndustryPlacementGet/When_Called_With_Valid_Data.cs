using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeIndustryPlacementGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public int PathwayId { get; set; }
        public IActionResult Result { get; private set; }
        protected AdminChangeIndustryPlacementViewModel AdminChangeIndustryPlacementViewModel = null;
        protected AdminChangeIndustryPlacementViewModel Mockresult = null;

        public override void Given()
      {
            PathwayId = 10;
            AdminChangeIndustryPlacementViewModel = new() { IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.Completed };
            Mockresult = new AdminChangeIndustryPlacementViewModel
            {
                Uln = 1235469874,
                FirstName = "firstname",
                LastName = "lastname",
                ProviderName = "provider-name",
                ProviderUkprn = 58794528,
                TlevelName = "t-level-name",
                AcademicYear = 2020,
                DisplayAcademicYear = "2021 to 2022",
                IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.Completed
            };

            AdminDashboardLoader.GetAdminLearnerRecordAsync<AdminChangeIndustryPlacementViewModel>(PathwayId).Returns(Mockresult);
        }

        public async override Task When()
        {
            Result = await Controller.ChangeIndustryPlacementAsync(PathwayId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.Received(1).GetAdminLearnerRecordAsync<AdminChangeIndustryPlacementViewModel>(PathwayId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as AdminChangeIndustryPlacementViewModel;

            model.PathwayId.Should().Be(Mockresult.PathwayId);
            model.Uln.Should().Be(Mockresult.Uln);
            model.Learner.Should().Be(Mockresult.Learner);
            model.FirstName.Should().Be(Mockresult.FirstName);
            model.LastName.Should().Be(Mockresult.LastName);
            model.ProviderName.Should().Be(Mockresult.ProviderName);
            model.ProviderUkprn.Should().Be(Mockresult.ProviderUkprn);
            model.TlevelName.Should().Be(Mockresult.TlevelName);
            model.DisplayAcademicYear.Should().Be("2021 to 2022");
            model.IndustryPlacementStatus.Should().Be(Common.Enum.IndustryPlacementStatus.Completed);

            // Learner
            model.SummaryLearner.Title.Should().Be(AdminChangeIndustryPlacement.Title_Learner_Text);
            model.SummaryLearner.Value.Should().Be(Mockresult.Learner);

            //Uln
            model.SummaryULN.Title.Should().Be(AdminChangeIndustryPlacement.Title_ULN_Text);
            model.SummaryULN.Value.Should().Be(Mockresult.Uln.ToString());

            // Provider
            model.SummaryProvider.Title.Should().Be(AdminChangeIndustryPlacement.Title_Provider_Text);
            model.SummaryProvider.Value.Should().Be($"{Mockresult.ProviderName} ({Mockresult.ProviderUkprn.ToString()})");

            // TLevelTitle
            model.SummaryTlevel.Title.Should().Be(AdminChangeIndustryPlacement.Title_TLevel_Text);
            model.SummaryTlevel.Value.Should().Be(Mockresult.TlevelName);

            // Start Year
            model.SummaryAcademicYear.Title.Should().Be(AdminChangeIndustryPlacement.Title_StartYear_Text);
            model.SummaryAcademicYear.Value.Should().Be(Mockresult.DisplayAcademicYear);

            // Start Year
            model.SummaryIndustryPlacementStatus.Title.Should().Be(AdminChangeIndustryPlacement.Title_Industry_Placement_Status);
            model.SummaryIndustryPlacementStatus.Value.Should().Be(Mockresult.GetIndustryPlacementDisplayText);

            // Back link
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AdminLearnerRecord);
        }
    }
}
