using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.SubjectsStatus;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangesMathsStatusGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        protected AdminReviewChangesMathsStatusViewModel _mockResult = null;

        public override void Given()
        {
            PathwayId = 5772;

            var adminChangeStatusViewModel = new AdminChangeMathsStatusViewModel
            {
                Uln = 123789555,
                LearnerName = "firstname",
                Provider = "provider-name",
                TlevelName = "t-level-name",
                MathsStatusTo = SubjectStatus.Achieved
            };

            _mockResult = new AdminReviewChangesMathsStatusViewModel
            {
                ContactName = "contact-name",
                Day = "01",
                Month = "01",
                Year = "1970",
                ChangeReason = "change-reason",
                ZendeskId = "1234567890",
                AdminChangeStatusViewModel = adminChangeStatusViewModel
            };

            AdminDashboardLoader.GetAdminLearnerRecordAsync<AdminReviewChangesMathsStatusViewModel>(PathwayId).Returns(_mockResult);
            CacheService.GetAsync<AdminChangeMathsStatusViewModel>(CacheKey).Returns(adminChangeStatusViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as AdminReviewChangesMathsStatusViewModel;

            model.AdminChangeStatusViewModel.RegistrationPathwayId.Should().Be(_mockResult.AdminChangeStatusViewModel.RegistrationPathwayId);
            model.AdminChangeStatusViewModel.Uln.Should().Be(_mockResult.AdminChangeStatusViewModel.Uln);
            model.AdminChangeStatusViewModel.Provider.Should().Be(_mockResult.AdminChangeStatusViewModel.Provider);
            model.AdminChangeStatusViewModel.TlevelName.Should().Be(_mockResult.AdminChangeStatusViewModel.TlevelName);

            model.AdminChangeStatusViewModel.SummaryLearner.Title.Should().Be(AdminChangeMathsStatus.Title_Learner_Text);
            model.AdminChangeStatusViewModel.SummaryLearner.Value.Should().Be(_mockResult.AdminChangeStatusViewModel.LearnerName);

            model.AdminChangeStatusViewModel.SummaryULN.Title.Should().Be(AdminChangeMathsStatus.Title_ULN_Text);
            model.AdminChangeStatusViewModel.SummaryULN.Value.Should().Be(_mockResult.AdminChangeStatusViewModel.Uln.ToString());

            model.AdminChangeStatusViewModel.SummaryProvider.Title.Should().Be(AdminChangeMathsStatus.Title_Provider_Text);
            model.AdminChangeStatusViewModel.SummaryProvider.Value.Should().Be(_mockResult.AdminChangeStatusViewModel.Provider);

            model.AdminChangeStatusViewModel.SummaryTlevel.Title.Should().Be(AdminChangeMathsStatus.Title_TLevel_Text);
            model.AdminChangeStatusViewModel.SummaryTlevel.Value.Should().Be(_mockResult.AdminChangeStatusViewModel.TlevelName);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AdminChangeMathsStatus);
        }
    }
}