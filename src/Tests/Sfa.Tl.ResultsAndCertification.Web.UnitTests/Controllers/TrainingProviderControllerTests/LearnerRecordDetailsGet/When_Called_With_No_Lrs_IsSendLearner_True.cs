using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System;
using System.Collections.Generic;
using Xunit;
using EnglishAndMathsStatusContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.EnglishAndMathsStatus;
using IndustryPlacementStatusContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.IndustryPlacementStatus;
using LearnerRecordDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.LearnerRecordDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.LearnerRecordDetailsGet
{
    public class When_Called_With_No_Lrs_IsSendLearner_True : TestSetup
    {
        public override void Given()
        {
            ProfileId = 10;
            Mockresult = new LearnerRecordDetailsViewModel
            {
                ProfileId = 10,
                RegistrationPathwayId = 15,
                Uln = 1235469874,
                Name = "Test user",
                DateofBirth = DateTime.UtcNow.AddYears(-20),
                ProviderName = "Barsley College (58794528)",
                PathwayName = "Test Pathway Name (97453214)",
                IsLearnerRegistered = true,
                IsLearnerRecordAdded = true,
                IsEnglishAndMathsAchieved = true,
                IsSendLearner = true,
                HasLrsEnglishAndMaths = false,
                IndustryPlacementId = 10,
                IndustryPlacementStatus = IndustryPlacementStatus.Completed
            };
            TrainingProviderLoader.GetLearnerRecordDetailsAsync<LearnerRecordDetailsViewModel>(ProviderUkprn, ProfileId).Returns(Mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            TrainingProviderLoader.Received(1).GetLearnerRecordDetailsAsync<LearnerRecordDetailsViewModel>(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as LearnerRecordDetailsViewModel;

            model.ProfileId.Should().Be(Mockresult.ProfileId);
            model.Uln.Should().Be(Mockresult.Uln);
            model.Name.Should().Be(Mockresult.Name);
            model.DateofBirth.Should().Be(Mockresult.DateofBirth);
            model.ProviderName.Should().Be(Mockresult.ProviderName);
            model.PathwayName.Should().Be(Mockresult.PathwayName);
            model.IsLearnerRegistered.Should().Be(Mockresult.IsLearnerRegistered);
            model.IsLearnerRecordAdded.Should().Be(Mockresult.IsLearnerRecordAdded);
            model.IsEnglishAndMathsAchieved.Should().Be(Mockresult.IsEnglishAndMathsAchieved);
            model.HasLrsEnglishAndMaths.Should().Be(Mockresult.HasLrsEnglishAndMaths);
            model.IsSendLearner.Should().Be(Mockresult.IsSendLearner);
            model.IndustryPlacementId.Should().Be(Mockresult.IndustryPlacementId);
            model.IndustryPlacementStatus.Should().Be(Mockresult.IndustryPlacementStatus);

            // Summary EnglishAndMathsStatus           
            model.SummaryEnglishAndMathsStatus.Should().NotBeNull();
            model.SummaryEnglishAndMathsStatus.Title.Should().Be(LearnerRecordDetailsContent.Title_EnglishAndMaths_Status_Text);
            model.SummaryEnglishAndMathsStatus.Value.Should().Be(EnglishAndMathsStatusContent.Achieved_With_Send_Display_Text);
            model.SummaryEnglishAndMathsStatus.NeedBorderBottomLine.Should().BeFalse();
            model.SummaryEnglishAndMathsStatus.RenderActionColumn.Should().BeTrue();
            model.SummaryEnglishAndMathsStatus.RenderHiddenActionText.Should().BeTrue();
            model.SummaryEnglishAndMathsStatus.HiddenActionText.Should().Be(LearnerRecordDetailsContent.English_And_Maths_Action_Hidden_Text);
            model.SummaryEnglishAndMathsStatus.ActionText.Should().Be(LearnerRecordDetailsContent.Update_Action_Link_Text);
            model.SummaryEnglishAndMathsStatus.RouteName.Should().Be(RouteConstants.UpdateEnglisAndMathsAchievement);
            model.SummaryEnglishAndMathsStatus.RouteAttributes.Should().BeEquivalentTo(GetEnglishAndMathsRouteAttributes);

            // Summary IndustryPlacementStatus
            model.SummaryIndustryPlacementStatus.Should().NotBeNull();
            model.SummaryIndustryPlacementStatus.Title.Should().Be(LearnerRecordDetailsContent.Title_IP_Status_Text);
            model.SummaryIndustryPlacementStatus.Value.Should().Be(IndustryPlacementStatusContent.Completed_Display_Text);
            model.SummaryIndustryPlacementStatus.NeedBorderBottomLine.Should().BeFalse();
            model.SummaryIndustryPlacementStatus.RenderActionColumn.Should().BeTrue();
            model.SummaryIndustryPlacementStatus.RenderHiddenActionText.Should().BeTrue();
            model.SummaryIndustryPlacementStatus.HiddenActionText.Should().Be(LearnerRecordDetailsContent.Industry_Placement_Action_Hidden_Text);
            model.SummaryIndustryPlacementStatus.ActionText.Should().Be(LearnerRecordDetailsContent.Update_Action_Link_Text);
            model.SummaryIndustryPlacementStatus.RouteName.Should().Be(RouteConstants.UpdateIndustryPlacementQuestion);
            model.SummaryIndustryPlacementStatus.RouteAttributes.Should().BeEquivalentTo(new Dictionary<string, string> { { Constants.ProfileId, Mockresult.ProfileId.ToString() }, { Constants.PathwayId, Mockresult.RegistrationPathwayId.ToString() } });
        }

        private Dictionary<string, string> GetEnglishAndMathsRouteAttributes => new Dictionary<string, string> { { Constants.ProfileId, Mockresult.ProfileId.ToString() } };
    }
}
