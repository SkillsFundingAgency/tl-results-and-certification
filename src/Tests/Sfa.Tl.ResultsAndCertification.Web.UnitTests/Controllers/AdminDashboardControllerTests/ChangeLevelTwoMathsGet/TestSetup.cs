using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.SubjectResults;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeLevelTwoMathsGet
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {
        protected const int RegistrationPathwayId = 999;
        protected IActionResult Result { get; private set; }

        protected static AdminChangeMathsResultsViewModel ViewModel 
            => new()
            {
                RegistrationPathwayId = RegistrationPathwayId,
                LearnerName = "Kevin Smith",
                Uln = 1234567890,
                Provider = "Barnsley College (10000536)",
                TlevelName = "Education and Early Years",
                AcademicYear = 2020,
                StartYear = "2021 to 2022",
                MathsStatus = SubjectStatus.NotAchieved
            };

        public async override Task When()
        {
            Result = await Controller.AdminChangeMathsStatusAsync(RegistrationPathwayId);
        }

        protected void AssertViewResult()
        {
            var model = Result.ShouldBeViewResult<AdminChangeMathsResultsViewModel>();
            var changeResultsModel = ViewModel;

            model.RegistrationPathwayId.Should().Be(changeResultsModel.RegistrationPathwayId);
            model.Uln.Should().Be(changeResultsModel.Uln);
            model.LearnerName.Should().Be(changeResultsModel.LearnerName);
            model.Provider.Should().Be(changeResultsModel.Provider);
            model.TlevelName.Should().Be(changeResultsModel.TlevelName);
            model.MathsStatus.Should().Be(changeResultsModel.MathsStatus);

            model.RegistrationPathwayId.Should().Be(changeResultsModel.RegistrationPathwayId);
            model.Uln.Should().Be(changeResultsModel.Uln);
            model.LearnerName.Should().Be(changeResultsModel.LearnerName);
            model.Provider.Should().Be(changeResultsModel.Provider);
            model.TlevelName.Should().Be(changeResultsModel.TlevelName);
            model.MathsStatus.Should().Be(changeResultsModel.MathsStatus);

            model.SummaryLearner.Title.Should().Be(AdminChangeLevelTwoMaths.Title_Learner_Text);
            model.SummaryLearner.Value.Should().Be(changeResultsModel.LearnerName);

            model.SummaryULN.Title.Should().Be(AdminChangeLevelTwoMaths.Title_ULN_Text);
            model.SummaryULN.Value.Should().Be(changeResultsModel.Uln.ToString());

            model.SummaryProvider.Title.Should().Be(AdminChangeLevelTwoMaths.Title_Provider_Text);
            model.SummaryProvider.Value.Should().Be(changeResultsModel.Provider);

            model.SummaryTlevel.Title.Should().Be(AdminChangeLevelTwoMaths.Title_TLevel_Text);
            model.SummaryTlevel.Value.Should().Be(changeResultsModel.TlevelName);

            model.SummaryAcademicYear.Title.Should().Be(AdminChangeLevelTwoMaths.Title_StartYear_Text);
            model.SummaryAcademicYear.Value.Should().Be(changeResultsModel.StartYear);

            model.SummaryMathsStatus.Title.Should().Be(AdminChangeLevelTwoMaths.Title_Maths_Status);
            model.SummaryMathsStatus.Value.Should().Be(changeResultsModel.GetSubjectStatusDisplayText(changeResultsModel.MathsStatus));

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AdminLearnerRecord);
        }
    }
}
