using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using System;
using System.Linq;
using Xunit;
using LearnerRecordDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.LearnerRecordDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TrainingProviderTests.GetLearnerRecordDetails
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private Models.Contracts.TrainingProvider.LearnerRecordDetails _expectedApiResult;
        
        public override void Given()
        {
            ProviderUkprn = 9874561231;
            ProfileId = 1;

            _expectedApiResult = new Models.Contracts.TrainingProvider.LearnerRecordDetails
            {
                ProfileId = ProfileId,
                Uln = 123456789,
                Name = "Test user",
                DateofBirth = DateTime.UtcNow.AddYears(-20),
                ProviderName = "Barsley College (12345678)",
                PathwayName = "Course name (4561237)",
                IsLearnerRegistered = true,
                IsLearnerRecordAdded = true,
                IsEnglishAndMathsAchieved = true,                
                HasLrsEnglishAndMaths = true,
                IsSendLearner = false,
                IndustryPlacementId = 1,
                IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.Completed
            };
            InternalApiClient.GetLearnerRecordDetailsAsync(ProviderUkprn, ProfileId).Returns(_expectedApiResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            InternalApiClient.Received(1).GetLearnerRecordDetailsAsync(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.ProfileId.Should().Be(_expectedApiResult.ProfileId);
            ActualResult.Uln.Should().Be(_expectedApiResult.Uln);
            ActualResult.Name.Should().Be(_expectedApiResult.Name);
            ActualResult.DateofBirth.Should().Be(_expectedApiResult.DateofBirth);
            ActualResult.ProviderName.Should().Be(_expectedApiResult.ProviderName);
            ActualResult.PathwayName.Should().Be(_expectedApiResult.PathwayName);
            ActualResult.IsLearnerRegistered.Should().Be(_expectedApiResult.IsLearnerRegistered);
            ActualResult.IsLearnerRecordAdded.Should().Be(_expectedApiResult.IsLearnerRecordAdded);
            ActualResult.IsEnglishAndMathsAchieved.Should().Be(_expectedApiResult.IsEnglishAndMathsAchieved);            
            ActualResult.HasLrsEnglishAndMaths.Should().Be(_expectedApiResult.HasLrsEnglishAndMaths);
            ActualResult.IsSendLearner.Should().Be(_expectedApiResult.IsSendLearner);
            ActualResult.IndustryPlacementId.Should().Be(_expectedApiResult.IndustryPlacementId);
            ActualResult.IndustryPlacementStatus.Should().Be(_expectedApiResult.IndustryPlacementStatus);

            var englishAndMathsStatus = ActualResult.SummaryEnglishAndMathsStatus;
            englishAndMathsStatus.Should().NotBeNull();
            englishAndMathsStatus.Id.Should().Be("englishmathsstatus");
            englishAndMathsStatus.Title.Should().Be(LearnerRecordDetailsContent.Title_EnglishAndMaths_Status_Text);
            englishAndMathsStatus.Value.Should().Be(string.Concat(GetLrsEnglishAndMathsStatusDisplayText, LearnerRecordDetailsContent.Whats_Lrs_Text));
            englishAndMathsStatus.ActionText.Should().Be(LearnerRecordDetailsContent.Query_Action_Link_Text);
            englishAndMathsStatus.RouteName.Should().Be(RouteConstants.QueryEnglishAndMathsStatus);
            englishAndMathsStatus.RouteAttributes.Count().Should().Be(1);
            englishAndMathsStatus.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileId);
            profileId.Should().Be(_expectedApiResult.ProfileId.ToString());
            englishAndMathsStatus.NeedBorderBottomLine.Should().BeFalse();
            englishAndMathsStatus.RenderHiddenActionText.Should().BeTrue();
            englishAndMathsStatus.HiddenActionText.Should().Be(LearnerRecordDetailsContent.English_And_Maths_Action_Hidden_Text);
        }
    }
}
