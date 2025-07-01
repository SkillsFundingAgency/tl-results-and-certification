using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.SubjectResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangesEnglishStatusGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        protected AdminReviewChangesEnglishSubjectViewModel _mockResult = null;

        public override void Given()
        {
            PathwayId = 5772;

            var adminChangeResultsViewModel = new AdminChangeEnglishResultsViewModel
            {
                Uln = 123789555,
                LearnerName = "firstname",
                Provider = "provider-name",
                TlevelName = "t-level-name",
                EnglishStatusTo = SubjectStatus.Achieved
            };

            _mockResult = new AdminReviewChangesEnglishSubjectViewModel
            {
                ContactName = "contact-name",
                Day = "01",
                Month = "01",
                Year = "1970",
                ChangeReason = "change-reason",
                ZendeskId = "1234567890",
                AdminChangeResultsViewModel = adminChangeResultsViewModel
            };

            AdminDashboardLoader.GetAdminLearnerRecordAsync<AdminReviewChangesEnglishSubjectViewModel>(PathwayId).Returns(_mockResult);
            CacheService.GetAsync<AdminChangeEnglishResultsViewModel>(CacheKey).Returns(adminChangeResultsViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as AdminReviewChangesEnglishSubjectViewModel;

            model.AdminChangeResultsViewModel.RegistrationPathwayId.Should().Be(_mockResult.AdminChangeResultsViewModel.RegistrationPathwayId);
            model.AdminChangeResultsViewModel.Uln.Should().Be(_mockResult.AdminChangeResultsViewModel.Uln);
            model.AdminChangeResultsViewModel.Provider.Should().Be(_mockResult.AdminChangeResultsViewModel.Provider);
            model.AdminChangeResultsViewModel.TlevelName.Should().Be(_mockResult.AdminChangeResultsViewModel.TlevelName);

            model.AdminChangeResultsViewModel.SummaryLearner.Title.Should().Be(AdminChangeLevelTwoEnglish.Title_Learner_Text);
            model.AdminChangeResultsViewModel.SummaryLearner.Value.Should().Be(_mockResult.AdminChangeResultsViewModel.LearnerName);

            model.AdminChangeResultsViewModel.SummaryULN.Title.Should().Be(AdminChangeLevelTwoEnglish.Title_ULN_Text);
            model.AdminChangeResultsViewModel.SummaryULN.Value.Should().Be(_mockResult.AdminChangeResultsViewModel.Uln.ToString());

            model.AdminChangeResultsViewModel.SummaryProvider.Title.Should().Be(AdminChangeLevelTwoEnglish.Title_Provider_Text);
            model.AdminChangeResultsViewModel.SummaryProvider.Value.Should().Be(_mockResult.AdminChangeResultsViewModel.Provider);

            model.AdminChangeResultsViewModel.SummaryTlevel.Title.Should().Be(AdminChangeLevelTwoEnglish.Title_TLevel_Text);
            model.AdminChangeResultsViewModel.SummaryTlevel.Value.Should().Be(_mockResult.AdminChangeResultsViewModel.TlevelName);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AdminChangeEnglishStatus);
        }
    }
}