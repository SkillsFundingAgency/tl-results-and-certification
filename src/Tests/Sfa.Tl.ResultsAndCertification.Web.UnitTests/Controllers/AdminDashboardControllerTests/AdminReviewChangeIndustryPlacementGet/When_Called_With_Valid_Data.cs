using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangeIndustryPlacementGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        protected AdminReviewChangesIndustryPlacementViewModel Mockresult = null;

        public override void Given()
        {
            PathwayId = 10;

            AdminIpCompletionViewModel adminIpCompletionViewModel = new()
            {
                Uln = 1235469874,
                LearnerName = "firstname",
                Provider = "provider-name",
                TlevelName = "t-level-name",
                IndustryPlacementStatusTo = Common.Enum.IndustryPlacementStatus.Completed
            };

            AdminChangeIpViewModel adminChangeIpViewModel = new()
            {
                AdminIpCompletion = adminIpCompletionViewModel,
                HoursViewModel = new(),
                ReasonsViewModel = new()
            };

            Mockresult = new AdminReviewChangesIndustryPlacementViewModel
            {
                ContactName = "contact-name",
                Day = "01",
                Month = "01",
                Year = "1970",
                ChangeReason = "change-reason",
                ZendeskId = "1234567890",
                AdminChangeIpViewModel = adminChangeIpViewModel
            };

            AdminDashboardLoader.GetAdminLearnerRecordAsync<AdminReviewChangesIndustryPlacementViewModel>(PathwayId).Returns(Mockresult);
            CacheService.GetAsync<AdminChangeIpViewModel>(CacheKey).Returns(adminChangeIpViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as AdminReviewChangesIndustryPlacementViewModel;

            model.AdminChangeIpViewModel.AdminIpCompletion.RegistrationPathwayId.Should().Be(Mockresult.AdminChangeIpViewModel.AdminIpCompletion.RegistrationPathwayId);
            model.AdminChangeIpViewModel.AdminIpCompletion.Uln.Should().Be(Mockresult.AdminChangeIpViewModel.AdminIpCompletion.Uln);
            model.AdminChangeIpViewModel.AdminIpCompletion.Provider.Should().Be(Mockresult.AdminChangeIpViewModel.AdminIpCompletion.Provider);
            model.AdminChangeIpViewModel.AdminIpCompletion.TlevelName.Should().Be(Mockresult.AdminChangeIpViewModel.AdminIpCompletion.TlevelName);

            // Learner
            model.AdminChangeIpViewModel.AdminIpCompletion.SummaryLearner.Title.Should().Be(ReviewChangesIndustryPlacement.Title_Learner_Text);
            model.AdminChangeIpViewModel.AdminIpCompletion.SummaryLearner.Value.Should().Be(Mockresult.AdminChangeIpViewModel.AdminIpCompletion.LearnerName);

            //Uln
            model.AdminChangeIpViewModel.AdminIpCompletion.SummaryULN.Title.Should().Be(ReviewChangesIndustryPlacement.Title_ULN_Text);
            model.AdminChangeIpViewModel.AdminIpCompletion.SummaryULN.Value.Should().Be(Mockresult.AdminChangeIpViewModel.AdminIpCompletion.Uln.ToString());

            // Provider
            model.AdminChangeIpViewModel.AdminIpCompletion.SummaryProvider.Title.Should().Be(ReviewChangesIndustryPlacement.Title_Provider_Text);
            model.AdminChangeIpViewModel.AdminIpCompletion.SummaryProvider.Value.Should().Be(Mockresult.AdminChangeIpViewModel.AdminIpCompletion.Provider);

            // TLevelTitle
            model.AdminChangeIpViewModel.AdminIpCompletion.SummaryTlevel.Title.Should().Be(ReviewChangesIndustryPlacement.Title_TLevel_Text);
            model.AdminChangeIpViewModel.AdminIpCompletion.SummaryTlevel.Value.Should().Be(Mockresult.AdminChangeIpViewModel.AdminIpCompletion.TlevelName);

            // Back link
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AdminChangeIndustryPlacement);
        }
    }
}
