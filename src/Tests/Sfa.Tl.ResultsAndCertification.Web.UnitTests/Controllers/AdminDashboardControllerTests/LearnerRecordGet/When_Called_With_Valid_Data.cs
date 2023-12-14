using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LearnerRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using LearnerRecordDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard.LearnerRecord;
using SubjectStatusContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.SubjectStatus;
using IndustryPlacement = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.IndustryPlacementStatus;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.LearnerRecordGet
{
    public class When_Called_With_Valid_Data:AdminDashboardControllerTestBase
    {
        private Dictionary<string, string> _routeAttributes;
        public int PathwayId { get; set; }
        public IActionResult Result { get; private set; }
        public AdminLearnerRecordViewModel AdminLearnerRecordViewModel;
        protected AdminLearnerRecordViewModel Mockresult = null;

        public override void Given()
        {
            PathwayId = 10;
            Mockresult = new AdminLearnerRecordViewModel
            {
                
                RegistrationPathwayId = 15,
                Uln = 1235469874,
                LearnerName = "John Smith",
                DateofBirth = DateTime.UtcNow.AddYears(-20),
                ProviderName = "Barnsley College",
                ProviderUkprn = 58794528,
                TlevelName = "Tlevel in Test Pathway Name",
                AcademicYear = 2021,
                AwardingOrganisationName = "NCFE",
                MathsStatus = SubjectStatus.NotSpecified,
                EnglishStatus = SubjectStatus.NotSpecified,
                IsLearnerRegistered = true,
                IndustryPlacementId = 10,
                IndustryPlacementStatus = IndustryPlacementStatus.Completed                
            };

            _routeAttributes = new Dictionary<string, string> { { Constants.PathwayId, Mockresult.RegistrationPathwayId.ToString() } };

            AdminDashboardLoader.GetAdminLearnerRecordAsync<AdminLearnerRecordViewModel>(PathwayId).Returns(Mockresult);
            ResultsAndCertificationConfiguration.DocumentRerequestInDays = 21;
        }

        public async override Task When()
        {
            Result = await Controller.AdminLearnerRecordAsync(PathwayId);
        }


        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.Received(1).GetAdminLearnerRecordAsync<AdminLearnerRecordViewModel>(PathwayId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as AdminLearnerRecordViewModel;

            model.ProfileId.Should().Be(Mockresult.ProfileId);
            model.RegistrationPathwayId.Should().Be(Mockresult.RegistrationPathwayId);
            model.Uln.Should().Be(Mockresult.Uln);
            model.LearnerName.Should().Be(Mockresult.LearnerName);
            model.DateofBirth.Should().Be(Mockresult.DateofBirth);
            model.ProviderName.Should().Be(Mockresult.ProviderName);
            model.ProviderUkprn.Should().Be(Mockresult.ProviderUkprn);
            model.TlevelName.Should().Be(Mockresult.TlevelName);
            model.StartYear.Should().Be("2021 to 2022");
            model.AwardingOrganisationName.Should().Be(Mockresult.AwardingOrganisationName);
            model.MathsStatus.Should().Be(Mockresult.MathsStatus);
            model.EnglishStatus.Should().Be(Mockresult.EnglishStatus);
            model.IsLearnerRegistered.Should().Be(Mockresult.IsLearnerRegistered);

            model.IndustryPlacementId.Should().Be(Mockresult.IndustryPlacementId);
            model.IndustryPlacementStatus.Should().Be(Mockresult.IndustryPlacementStatus);

            model.IsMathsAdded.Should().BeFalse();
            model.IsEnglishAdded.Should().BeFalse();
            model.IsIndustryPlacementAdded.Should().BeTrue();
            model.IsStatusCompleted.Should().BeFalse();
          

            // DateofBirth
            model.SummaryDateofBirth.Title.Should().Be(LearnerRecordDetailsContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be(Mockresult.DateofBirth.ToDobFormat());

            // ProviderName
           
            model.SummaryProviderName.Value.Should().Be(Mockresult.ProviderName+ " (" + Mockresult.ProviderUkprn.ToString()+")");

            // ProviderUkprn
           model.SummaryProviderUkprn.Value.Should().Be(Mockresult.ProviderUkprn.ToString());

            // TLevelTitle
            model.SummaryTlevelTitle.Title.Should().Be(LearnerRecordDetailsContent.Title_TLevel_Text);
            model.SummaryTlevelTitle.Value.Should().Be(Mockresult.TlevelName);

            // Start Year
            model.SummaryStartYear.Title.Should().Be(LearnerRecordDetailsContent.Title_StartYear_Text);
            model.SummaryStartYear.Value.Should().Be(Mockresult.StartYear);

            // AO Name
            model.SummaryAoName.Title.Should().Be(LearnerRecordDetailsContent.Title_AoName_Text);
            model.SummaryAoName.Value.Should().Be(Mockresult.AwardingOrganisationName);

            // Summary Industry Placement
            model.SummaryIndustryPlacementStatus.Should().NotBeNull();
            model.SummaryIndustryPlacementStatus.Title.Should().Be(LearnerRecordDetailsContent.Title_IP_Status_Text);
            model.SummaryIndustryPlacementStatus.Value.Should().Be(IndustryPlacement.Completed_Display_Text);
            model.SummaryIndustryPlacementStatus.HiddenActionText.Should().Be(LearnerRecordDetailsContent.Hidden_Action_Text_Industry_Placement);
            model.SummaryIndustryPlacementStatus.ActionText.Should().Be(LearnerRecordDetailsContent.Action_Text_Link_Change);
            
            // Summary Maths StatusHidden_Action_Text_Maths
            model.SummaryMathsStatus.Should().NotBeNull();
            model.SummaryMathsStatus.Title.Should().Be(LearnerRecordDetailsContent.Title_Maths_Text);
            model.SummaryMathsStatus.Value.Should().Be(SubjectStatusContent.Not_Yet_Recevied_Display_Text);
            

            // Summary English Status
            model.SummaryEnglishStatus.Should().NotBeNull();
            model.SummaryEnglishStatus.Title.Should().Be(LearnerRecordDetailsContent.Title_English_Text);
            model.SummaryMathsStatus.Value.Should().Be(SubjectStatusContent.Not_Yet_Recevied_Display_Text);
           
            // Back link
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AdminSearchLearnersRecords);
        }




    }
}
