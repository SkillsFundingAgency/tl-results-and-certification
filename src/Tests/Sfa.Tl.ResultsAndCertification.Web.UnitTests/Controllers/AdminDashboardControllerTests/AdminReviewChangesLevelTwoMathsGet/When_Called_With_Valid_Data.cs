using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LevelTwoResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangesLevelTwoMathsGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        protected AdminReviewChangesMathsSubjectViewModel _mockResult = null;

        public override void Given()
        {
            PathwayId = 5772;

            var adminChangeResultsViewModel = new AdminChangeMathsResultsViewModel
            {
                Uln = 123789555,
                LearnerName = "firstname",
                Provider = "provider-name",
                TlevelName = "t-level-name",
                MathsStatusTo = SubjectStatus.Achieved
            };

            _mockResult = new AdminReviewChangesMathsSubjectViewModel
            {
                ContactName = "contact-name",
                Day = "01",
                Month = "01",
                Year = "1970",
                ChangeReason = "change-reason",
                ZendeskId = "1234567890",
                AdminChangeResultsViewModel = adminChangeResultsViewModel
            };

            AdminDashboardLoader.GetAdminLearnerRecordAsync<AdminReviewChangesMathsSubjectViewModel>(PathwayId).Returns(_mockResult);
            CacheService.GetAsync<AdminChangeMathsResultsViewModel>(CacheKey).Returns(adminChangeResultsViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as AdminReviewChangesMathsSubjectViewModel;

            model.AdminChangeResultsViewModel.RegistrationPathwayId.Should().Be(_mockResult.AdminChangeResultsViewModel.RegistrationPathwayId);
            model.AdminChangeResultsViewModel.Uln.Should().Be(_mockResult.AdminChangeResultsViewModel.Uln);
            model.AdminChangeResultsViewModel.Provider.Should().Be(_mockResult.AdminChangeResultsViewModel.Provider);
            model.AdminChangeResultsViewModel.TlevelName.Should().Be(_mockResult.AdminChangeResultsViewModel.TlevelName);

            model.AdminChangeResultsViewModel.SummaryLearner.Title.Should().Be(AdminChangeLevelTwoMaths.Title_Learner_Text);
            model.AdminChangeResultsViewModel.SummaryLearner.Value.Should().Be(_mockResult.AdminChangeResultsViewModel.LearnerName);

            model.AdminChangeResultsViewModel.SummaryULN.Title.Should().Be(AdminChangeLevelTwoMaths.Title_ULN_Text);
            model.AdminChangeResultsViewModel.SummaryULN.Value.Should().Be(_mockResult.AdminChangeResultsViewModel.Uln.ToString());

            model.AdminChangeResultsViewModel.SummaryProvider.Title.Should().Be(AdminChangeLevelTwoMaths.Title_Provider_Text);
            model.AdminChangeResultsViewModel.SummaryProvider.Value.Should().Be(_mockResult.AdminChangeResultsViewModel.Provider);

            model.AdminChangeResultsViewModel.SummaryTlevel.Title.Should().Be(AdminChangeLevelTwoMaths.Title_TLevel_Text);
            model.AdminChangeResultsViewModel.SummaryTlevel.Value.Should().Be(_mockResult.AdminChangeResultsViewModel.TlevelName);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AdminChangeMathsStatus);
        }
    }
}