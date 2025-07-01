using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.SubjectResults;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeEnglishStatusGet
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {
        protected const int RegistrationPathwayId = 1;
        protected IActionResult Result { get; private set; }

        protected static AdminChangeEnglishResultsViewModel ViewModel
            => new()
            {
                RegistrationPathwayId = RegistrationPathwayId,
                LearnerName = "Kevin Smith",
                Uln = 1234567890,
                Provider = "Barnsley College (10000536)",
                TlevelName = "Education and Early Years",
                AcademicYear = 2020,
                StartYear = "2021 to 2022",
                EnglishStatus = SubjectStatus.NotAchieved
            };

        public async override Task When()
        {
            Result = await Controller.AdminChangeEnglishStatusAsync(RegistrationPathwayId);
        }

        protected void AssertViewResult()
        {
            var model = Result.ShouldBeViewResult<AdminChangeEnglishResultsViewModel>();
            var changeResultsModel = ViewModel;

            model.RegistrationPathwayId.Should().Be(changeResultsModel.RegistrationPathwayId);
            model.Uln.Should().Be(changeResultsModel.Uln);
            model.LearnerName.Should().Be(changeResultsModel.LearnerName);
            model.Provider.Should().Be(changeResultsModel.Provider);
            model.TlevelName.Should().Be(changeResultsModel.TlevelName);
            model.EnglishStatus.Should().Be(changeResultsModel.EnglishStatus);

            model.SummaryLearner.Title.Should().Be(AdminChangeEnglishStatus.Title_Learner_Text);
            model.SummaryLearner.Value.Should().Be(changeResultsModel.LearnerName);

            model.SummaryULN.Title.Should().Be(AdminChangeEnglishStatus.Title_ULN_Text);
            model.SummaryULN.Value.Should().Be(changeResultsModel.Uln.ToString());

            model.SummaryProvider.Title.Should().Be(AdminChangeEnglishStatus.Title_Provider_Text);
            model.SummaryProvider.Value.Should().Be(changeResultsModel.Provider);

            model.SummaryTlevel.Title.Should().Be(AdminChangeEnglishStatus.Title_TLevel_Text);
            model.SummaryTlevel.Value.Should().Be(changeResultsModel.TlevelName);

            model.SummaryAcademicYear.Title.Should().Be(AdminChangeEnglishStatus.Title_StartYear_Text);
            model.SummaryAcademicYear.Value.Should().Be(changeResultsModel.StartYear);

            model.SummaryEnglishStatus.Title.Should().Be(AdminChangeEnglishStatus.Title_English_Status);
            model.SummaryEnglishStatus.Value.Should().Be(changeResultsModel.GetSubjectStatusDisplayText(changeResultsModel.EnglishStatus));

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AdminLearnerRecord);
        }
    }
}
