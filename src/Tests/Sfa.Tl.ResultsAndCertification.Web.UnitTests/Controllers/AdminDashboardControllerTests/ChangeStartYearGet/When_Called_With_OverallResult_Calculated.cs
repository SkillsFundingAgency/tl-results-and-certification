﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeStartYearGet
{
    public class When_Called_With_OverallResult_Calculated : AdminDashboardControllerTestBase
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
                TlevelName = "Education and Early Years",
                AcademicYear = 2022,
                TlevelStartYear = 2020,
                DisplayAcademicYear = "2022 to 2023",
                AcademicStartYearsToBe = new List<int>() { 2021, 2020 },
                LearnerRegistrationPathwayStatus = "Active",
                OverallCalculationStatus=CalculationStatus.Completed
            };

            AdminDashboardLoader.GetAdminLearnerRecordAsync<AdminChangeStartYearViewModel>(PathwayId).Returns(Mockresult);
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

            model.ProfileId.Should().Be(Mockresult.ProfileId);
            model.Uln.Should().Be(Mockresult.Uln);
            model.Learner.Should().Be(Mockresult.Learner);
            model.FirstName.Should().Be(Mockresult.FirstName);
            model.LastName.Should().Be(Mockresult.LastName);
            model.ProviderName.Should().Be(Mockresult.ProviderName);
            model.ProviderUkprn.Should().Be(Mockresult.ProviderUkprn);
            model.TlevelName.Should().Be(Mockresult.TlevelName);
            model.DisplayAcademicYear.Should().Be("2022 to 2023");

            model.AcademicStartYearsToBe.Should().NotBeEmpty();
            model.AcademicStartYearsToBe.Count.Should().Be(2);
            //model.AcademicStartYearsToBe.Should().Contain(new List<int>() { 2021, 2020 });

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

            model.OverallCalculationStatus.Should().Be(CalculationStatus.Completed);
            model.IsTlevelStartedSameAsStartYear.Should().BeFalse();
            model.IsLearnerWithdrawn.Should().BeFalse();
            model.StartYearCannotChangeMessage.Should().Be(ChangeStartYear.Message_Start_Year_Cannot_Be_Changed_Overall_Result_Already_Calculated);
        }

    }
}
