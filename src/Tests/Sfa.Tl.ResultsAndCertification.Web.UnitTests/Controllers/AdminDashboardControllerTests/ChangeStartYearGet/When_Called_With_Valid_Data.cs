using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LearnerRecord;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using LearnerRecordDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard.LearnerRecord;
using SubjectStatusContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.SubjectStatus;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeStartYearGet
{
    public class When_Called_With_Valid_Data:AdminDashboardControllerTestBase
    {
        public int PathwayId { get; set; }
        public IActionResult Result { get; private set; }
        public AdminChangeStartYearViewModel AdminChangeStartYearViewModel;
        protected AdminChangeStartYearViewModel Mockresult = null;

        public override void Given()
        {
            PathwayId = 10;
            Mockresult = new AdminChangeStartYearViewModel
            {
                Uln = 1235469874,
                FirstName = "John",
                LastName = "Smith",
                ProviderName = "Barnsley College",
                ProviderUkprn = 58794528,
                TlevelName = "Tlevel in Test Pathway Name",
                AcademicYear = 2022,
                TlevelStartYear = 2020,
                DisplayAcademicYear = "2021 to 2022",
                AcademicStartYearsToBe = new List<int>() { 2021, 2020}
            };

            AdminDashboardLoader.GetAdminLearnerRecordAsync<AdminChangeStartYearViewModel>(Arg.Any<int>()).Returns(Mockresult);
        }

        public async override Task When()
        {
            Result = await Controller.ChangeStartYearAsync(PathwayId);
        }


        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.Received(1).GetAdminLearnerRecordAsync<AdminChangeStartYearViewModel>(PathwayId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as AdminChangeStartYearViewModel;

            model.PathwayId.Should().Be(Mockresult.PathwayId);
            model.Uln.Should().Be(Mockresult.Uln);
            model.Learner.Should().Be(Mockresult.Learner);
            model.FirstName.Should().Be(Mockresult.FirstName);
            model.LastName.Should().Be(Mockresult.LastName);
            model.ProviderName.Should().Be(Mockresult.ProviderName);
            model.ProviderUkprn.Should().Be(Mockresult.ProviderUkprn);
            model.TlevelName.Should().Be(Mockresult.TlevelName);
            model.DisplayAcademicYear.Should().Be("2021 to 2022");

            model.AcademicStartYearsToBe.Should().NotBeEmpty();
            model.AcademicStartYearsToBe.Count.Should() .Be(2);
            model.AcademicStartYearsToBe.Should().Contain(new List<int>() { 2021, 2020 });

            // Learner
            model.SummaryLearner.Title.Should().Be(ChangeStartYear.Title_Learner_Text);
            model.SummaryLearner.Value.Should().Be(Mockresult.Learner);

            //Uln
            model.SummaryULN.Title.Should().Be(ChangeStartYear.Title_ULN_Text);
            model.SummaryULN.Value.Should().Be(Mockresult.Uln.ToString());

            // Provider
            model.SummaryProvider.Title.Should().Be(ChangeStartYear.Title_Provider_Text);
            model.SummaryProvider.Value.Should().Be($"{Mockresult.ProviderName} ({Mockresult.ProviderUkprn.ToString()})");

            // TLevelTitle
            model.SummaryTlevel.Title.Should().Be(ChangeStartYear.Title_TLevel_Text);
            model.SummaryTlevel.Value.Should().Be(Mockresult.TlevelName);

            // Start Year
            model.SummaryAcademicYear.Title.Should().Be(ChangeStartYear.Title_StartYear_Text);
            model.SummaryAcademicYear.Value.Should().Be(Mockresult.DisplayAcademicYear);
           
            // Back link
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AdminLearnerRecord);
        }




    }
}
