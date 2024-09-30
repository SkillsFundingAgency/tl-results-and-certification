using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeStartYearGet
{
    public class When_Called_With_AcademicYear_SameAs_TLevelStartYear : AdminDashboardControllerTestBase
    {
        private const int PathwayId = 10;
        private IActionResult _result;

        private AdminChangeStartYearViewModel _mockResult;

        public override void Given()
        {
            _mockResult = new AdminChangeStartYearViewModel
            {
                Uln = 1235469874,
                FirstName = "John",
                LastName = "Smith",
                ProviderName = "Barnsley College",
                ProviderUkprn = 58794528,
                TlevelName = "Education and Early Years",
                AcademicYear = 2020,
                TlevelStartYear = 2020,
                DisplayAcademicYear = "2020 to 2021",
                AcademicStartYearsToBe = new List<int>() { 2021, 2020 },
                LearnerRegistrationPathwayStatus = "Active"
            };

            AdminDashboardLoader.GetAdminLearnerRecordChangeYearAsync(PathwayId).Returns(_mockResult);
        }

        public async override Task When()
        {
            _result = await Controller.ChangeStartYearAsync(PathwayId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.Received(1).GetAdminLearnerRecordChangeYearAsync(PathwayId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            (_result as ViewResult).Model.Should().NotBeNull();

            var model = (_result as ViewResult).Model as AdminChangeStartYearViewModel;

            model.ProfileId.Should().Be(_mockResult.ProfileId);
            model.Uln.Should().Be(_mockResult.Uln);
            model.Learner.Should().Be(_mockResult.Learner);
            model.FirstName.Should().Be(_mockResult.FirstName);
            model.LastName.Should().Be(_mockResult.LastName);
            model.ProviderName.Should().Be(_mockResult.ProviderName);
            model.ProviderUkprn.Should().Be(_mockResult.ProviderUkprn);
            model.TlevelName.Should().Be(_mockResult.TlevelName);
            model.DisplayAcademicYear.Should().Be("2020 to 2021");

            model.AcademicStartYearsToBe.Should().NotBeEmpty();
            model.AcademicStartYearsToBe.Count.Should().Be(2);
            model.AcademicStartYearsToBe.Should().Contain(new[] { 2021, 2020 });

            // Learner
            model.SummaryLearner.Title.Should().Be(ChangeStartYear.Title_Learner_Text);
            model.SummaryLearner.Value.Should().Be(_mockResult.Learner);

            //Uln
            model.SummaryULN.Title.Should().Be(ChangeStartYear.Title_ULN_Text);
            model.SummaryULN.Value.Should().Be(_mockResult.Uln.ToString());

            // Provider
            model.SummaryProvider.Title.Should().Be(ChangeStartYear.Title_Provider_Text);
            model.SummaryProvider.Value.Should().Be($"{_mockResult.ProviderName} ({_mockResult.ProviderUkprn})");

            // TLevelTitle
            model.SummaryTlevel.Title.Should().Be(ChangeStartYear.Title_TLevel_Text);
            model.SummaryTlevel.Value.Should().Be(_mockResult.TlevelName);

            // Start Year
            model.SummaryAcademicYear.Title.Should().Be(ChangeStartYear.Title_StartYear_Text);
            model.SummaryAcademicYear.Value.Should().Be(_mockResult.DisplayAcademicYear);

            // Back link
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AdminLearnerRecord);

            model.IsLearnerWithdrawn.Should().BeFalse();
            model.StartYearCannotChangeMessage.Should().Be(ChangeStartYear.Message_Start_Year_Cannot_Be_Changed_Tlevel_Became_Available_This_Academic_Year);
        }
    }
}