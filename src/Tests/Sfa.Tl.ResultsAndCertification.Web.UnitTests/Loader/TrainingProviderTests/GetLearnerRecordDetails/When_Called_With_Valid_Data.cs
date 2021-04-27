using FluentAssertions;
using NSubstitute;
using System;
using Xunit;

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
        }
    }
}
