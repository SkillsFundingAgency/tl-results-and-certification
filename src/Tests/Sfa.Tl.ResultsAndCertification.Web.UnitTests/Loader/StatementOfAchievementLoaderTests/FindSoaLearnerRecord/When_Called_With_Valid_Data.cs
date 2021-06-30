using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.StatementOfAchievementLoaderTests.FindSoaLearnerRecord
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private Models.Contracts.StatementOfAchievement.FindSoaLearnerRecord _expectedApiResult;

        public override void Given()
        {
            _expectedApiResult = new Models.Contracts.StatementOfAchievement.FindSoaLearnerRecord
            {
                ProfileId = 1,
                Uln = 123456789,
                LearnerName = "Test user",
                DateofBirth = DateTime.UtcNow.AddYears(-20),
                ProviderName = "Barsley College (54678945)",
                TlevelTitle = "Title",
                Status = Common.Enum.RegistrationPathwayStatus.Active,
                IsIndustryPlacementAdded = true, 
                IndustryPlacementStatus = IndustryPlacementStatus.NotCompleted,
                HasPathwayResult = true
            };

            InternalApiClient.FindSoaLearnerRecordAsync(ProviderUkprn, Uln).Returns(_expectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.ProfileId.Should().Be(_expectedApiResult.ProfileId);
            ActualResult.Uln.Should().Be(_expectedApiResult.Uln);
            ActualResult.LearnerName.Should().Be(_expectedApiResult.LearnerName);
            ActualResult.DateofBirth.Should().Be(_expectedApiResult.DateofBirth);
            ActualResult.ProviderName.Should().Be(_expectedApiResult.ProviderName);
            ActualResult.TlevelTitle.Should().Be(_expectedApiResult.TlevelTitle);
            ActualResult.Status.Should().Be(_expectedApiResult.Status);
            ActualResult.IsLearnerRegistered.Should().BeTrue();
            ActualResult.IsIndustryPlacementAdded.Should().Be(_expectedApiResult.IsIndustryPlacementAdded);
            ActualResult.IndustryPlacementStatus.Should().Be(_expectedApiResult.IndustryPlacementStatus);
            ActualResult.HasPathwayResult.Should().Be(_expectedApiResult.HasPathwayResult);
        }
    }
}
