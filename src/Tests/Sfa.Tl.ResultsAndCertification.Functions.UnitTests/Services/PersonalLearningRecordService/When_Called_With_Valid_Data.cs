using FluentAssertions;
using Lrs.PersonalLearningRecordService.Api.Client;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.PersonalLearningRecordService
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private List<RegistrationLearnerDetails> _registrationLearnerDetails;
        private LearnerVerificationAndLearningEventsResponse _expectedResult;

        public override void Given()
        {
            var registrationLearnerDetails = new RegistrationLearnerDetails
            {
                ProfileId = 1,
                Uln = 1234567890,
                Firstname = "Test First 1",
                Lastname = "Test Last 1",
                DateofBirth = DateTime.UtcNow.AddYears(-40)
            };

            _registrationLearnerDetails = new List<RegistrationLearnerDetails> { registrationLearnerDetails };
            LearnerRecordService.GetPendingVerificationAndLearningEventsLearners().Returns(_registrationLearnerDetails);

            var apiResponse = new GetLearnerLearningEventsResponse { IncomingULN = registrationLearnerDetails.Uln.ToString(), FoundULN = registrationLearnerDetails.Uln.ToString() };
            PersonalLearningRecordApiClient.GetLearnerEventsAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime>()).Returns(apiResponse);

            _expectedResult = new LearnerVerificationAndLearningEventsResponse { IsSuccess = true, RegistrationsRecordsCount = 1, LrsRecordsCount = 1, ModifiedRecordsCount = 1, SavedRecordsCount = 1  };
            LearnerRecordService.ProcessLearnerRecords(Arg.Any<List<LearnerRecordDetails>>()).Returns(_expectedResult);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            LearnerRecordService.Received(1).GetPendingVerificationAndLearningEventsLearners();
            PersonalLearningRecordApiClient.Received(1).GetLearnerEventsAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime>());
            LearnerRecordService.Received(1).ProcessLearnerRecords(Arg.Any<List<LearnerRecordDetails>>());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.IsSuccess.Should().Be(_expectedResult.IsSuccess);
            ActualResult.RegistrationsRecordsCount.Should().Be(_expectedResult.RegistrationsRecordsCount);
            ActualResult.LrsRecordsCount.Should().Be(_expectedResult.LrsRecordsCount);
            ActualResult.ModifiedRecordsCount.Should().Be(_expectedResult.ModifiedRecordsCount);
            ActualResult.SavedRecordsCount.Should().Be(_expectedResult.SavedRecordsCount);
        }
    }
}
