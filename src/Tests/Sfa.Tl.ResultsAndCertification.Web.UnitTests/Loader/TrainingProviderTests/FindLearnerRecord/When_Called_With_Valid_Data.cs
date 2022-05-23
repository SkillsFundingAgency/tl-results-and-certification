using FluentAssertions;
using NSubstitute;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TrainingProviderTests.FindLearnerRecord
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private Models.Contracts.TrainingProvider.FindLearnerRecord expectedApiResult;

        public override void Given()
        {
            expectedApiResult = new Models.Contracts.TrainingProvider.FindLearnerRecord
            {
                ProfileId = 1,
                Uln = 123456789,
                Name = "Test user",
                DateofBirth = DateTime.UtcNow.AddYears(-20),
                ProviderName = "Barsley College",
                IsLearnerRegistered = true
            };

            InternalApiClient.FindLearnerRecordAsync(ProviderUkprn, Uln).Returns(expectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.ProfileId.Should().Be(expectedApiResult.ProfileId);
            ActualResult.Uln.Should().Be(expectedApiResult.Uln);
            ActualResult.Name.Should().Be(expectedApiResult.Name);
            ActualResult.DateofBirth.Should().Be(expectedApiResult.DateofBirth);
            ActualResult.ProviderName.Should().Be(expectedApiResult.ProviderName);
            ActualResult.IsLearnerRegistered.Should().Be(expectedApiResult.IsLearnerRegistered);
        }
    }
}
