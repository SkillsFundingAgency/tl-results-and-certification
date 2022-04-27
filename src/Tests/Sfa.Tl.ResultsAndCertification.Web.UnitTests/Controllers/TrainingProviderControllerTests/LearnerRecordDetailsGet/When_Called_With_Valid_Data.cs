using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System;
using Xunit;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using SubjectStatusContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.SubjectStatus;
using LearnerRecordDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.LearnerRecordDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.LearnerRecordDetailsGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            ProfileId = 10;
            Mockresult = new LearnerRecordDetailsViewModel1
            {
                ProfileId = 10,
                RegistrationPathwayId = 15,
                Uln = 1235469874,
                LearnerName = "Test user",
                DateofBirth = DateTime.UtcNow.AddYears(-20),
                ProviderName = "Barsley College",
                ProviderUkprn = 58794528,
                TlevelTitle = "Tlevel in Test Pathway Name",
                StartYear = "2020 to 2021",
                AwardingOrganisationName = "Pearson",
                MathsStatus = SubjectStatus.NotSpecified,
                EnglishStatus = SubjectStatus.NotSpecified,
                IsLearnerRegistered = true,
                
                IndustryPlacementId = 10,
                IndustryPlacementStatus = IndustryPlacementStatus.NotSpecified
            };
            TrainingProviderLoader.GetLearnerRecordDetailsAsync<LearnerRecordDetailsViewModel1>(ProviderUkprn, ProfileId).Returns(Mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            TrainingProviderLoader.Received(1).GetLearnerRecordDetailsAsync<LearnerRecordDetailsViewModel1>(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as LearnerRecordDetailsViewModel1;

            model.ProfileId.Should().Be(Mockresult.ProfileId);
            model.RegistrationPathwayId.Should().Be(Mockresult.RegistrationPathwayId);
            model.Uln.Should().Be(Mockresult.Uln);
            model.LearnerName.Should().Be(Mockresult.LearnerName);
            model.DateofBirth.Should().Be(Mockresult.DateofBirth);
            model.ProviderName.Should().Be(Mockresult.ProviderName);
            model.ProviderUkprn.Should().Be(Mockresult.ProviderUkprn);
            model.TlevelTitle.Should().Be(Mockresult.TlevelTitle);
            model.StartYear.Should().Be(Mockresult.StartYear);
            model.AwardingOrganisationName.Should().Be(Mockresult.AwardingOrganisationName);
            model.MathsStatus.Should().Be(Mockresult.MathsStatus);
            model.EnglishStatus.Should().Be(Mockresult.EnglishStatus);
            model.IsLearnerRegistered.Should().Be(Mockresult.IsLearnerRegistered);

            model.IndustryPlacementId.Should().Be(Mockresult.IndustryPlacementId);
            model.IndustryPlacementStatus.Should().Be(Mockresult.IndustryPlacementStatus);

            model.IsMathsAdded.Should().BeFalse();
            model.IsEnglishAdded.Should().BeFalse();
            model.IsIndustryPlacementAdded.Should().BeFalse();
            model.IsStatusCompleted.Should().BeFalse();

            // DateofBirth
            model.SummaryDateofBirth.Title.Should().Be(LearnerRecordDetailsContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be(Mockresult.DateofBirth.ToDobFormat());

            // ProviderName
            model.SummaryProviderName.Title.Should().Be(LearnerRecordDetailsContent.Title_Provider_Name_Text);
            model.SummaryProviderName.Value.Should().Be(Mockresult.ProviderName);

            // ProviderUkprn
            model.SummaryProviderUkprn.Title.Should().Be(LearnerRecordDetailsContent.Title_Provider_Ukprn_Text);
            model.SummaryProviderUkprn.Value.Should().Be(Mockresult.ProviderUkprn.ToString());

            // TLevelTitle
            model.SummaryTlevelTitle.Title.Should().Be(LearnerRecordDetailsContent.Title_TLevel_Text);
            model.SummaryTlevelTitle.Value.Should().Be(Mockresult.TlevelTitle);
            
            // Start Year
            model.SummaryStartYear.Title.Should().Be(LearnerRecordDetailsContent.Title_StartYear_Text);
            model.SummaryStartYear.Value.Should().Be(Mockresult.StartYear);

            // AO Name
            model.SummaryAoName.Title.Should().Be(LearnerRecordDetailsContent.Title_AoName_Text);
            model.SummaryAoName.Value.Should().Be(Mockresult.AwardingOrganisationName);

            // Summary Maths StatusHidden_Action_Text_Maths
            model.SummaryMathsStatus.Should().NotBeNull();
            model.SummaryMathsStatus.Title.Should().Be(LearnerRecordDetailsContent.Title_Maths_Text);
            model.SummaryMathsStatus.Value.Should().Be(SubjectStatusContent.Not_Yet_Recevied_Display_Text);
            model.SummaryMathsStatus.NeedBorderBottomLine.Should().BeTrue();
            model.SummaryMathsStatus.HiddenActionText.Should().Be(LearnerRecordDetailsContent.Hidden_Action_Text_Maths);
            model.SummaryMathsStatus.ActionText.Should().Be(LearnerRecordDetailsContent.Action_Text_Link_Add);

            // Summary English Status
            model.SummaryEnglishStatus.Should().NotBeNull();
            model.SummaryEnglishStatus.Title.Should().Be(LearnerRecordDetailsContent.Title_English_Text);
            model.SummaryMathsStatus.Value.Should().Be(SubjectStatusContent.Not_Yet_Recevied_Display_Text);
            model.SummaryEnglishStatus.NeedBorderBottomLine.Should().BeTrue();
            model.SummaryEnglishStatus.HiddenActionText.Should().Be(LearnerRecordDetailsContent.Hidden_Action_Text_English);
            model.SummaryEnglishStatus.ActionText.Should().Be(LearnerRecordDetailsContent.Action_Text_Link_Add);

            // Back link
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.SearchLearnerRecord);
        }
    }
}
