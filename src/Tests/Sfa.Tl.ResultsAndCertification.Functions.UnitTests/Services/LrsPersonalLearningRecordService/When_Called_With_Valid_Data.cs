﻿using FluentAssertions;
using Lrs.PersonalLearningRecordService.Api.Client;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.LrsPersonalLearningRecordService
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private List<RegisteredLearnerDetails> _registrationLearnerDetails;
        private LrsLearnerVerificationAndLearningEventsResponse _expectedResult;

        public override void Given()
        {
            var registrationLearnerDetails = new RegisteredLearnerDetails
            {
                ProfileId = 1,
                Uln = 1234567890,
                Firstname = "Test First 1",
                Lastname = "Test Last 1",
                DateofBirth = DateTime.UtcNow.AddYears(-40)
            };

            _registrationLearnerDetails = new List<RegisteredLearnerDetails> { registrationLearnerDetails };
            LrsService.GetPendingVerificationAndLearningEventsLearnersAsync().Returns(_registrationLearnerDetails);

            var apiResponse = new GetLearnerLearningEventsResponse { IncomingULN = registrationLearnerDetails.Uln.ToString(), FoundULN = registrationLearnerDetails.Uln.ToString() };
            LrsPersonalLearningRecordApiClient.GetLearnerEventsAsync(Arg.Any<RegisteredLearnerDetails>()).Returns(apiResponse);

            _expectedResult = new LrsLearnerVerificationAndLearningEventsResponse { IsSuccess = true, TotalCount = 1, LrsCount = 1, ModifiedCount = 1, SavedCount = 1  };
            LrsService.ProcessLearnerRecordsAsync(Arg.Any<List<LrsLearnerRecordDetails>>()).Returns(_expectedResult);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            LrsService.Received(1).GetPendingVerificationAndLearningEventsLearnersAsync();
            LrsPersonalLearningRecordApiClient.Received(1).GetLearnerEventsAsync(Arg.Any<RegisteredLearnerDetails>());
            LrsService.Received(1).ProcessLearnerRecordsAsync(Arg.Any<List<LrsLearnerRecordDetails>>());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.IsSuccess.Should().Be(_expectedResult.IsSuccess);
            ActualResult.TotalCount.Should().Be(_expectedResult.TotalCount);
            ActualResult.LrsCount.Should().Be(_expectedResult.LrsCount);
            ActualResult.ModifiedCount.Should().Be(_expectedResult.ModifiedCount);
            ActualResult.SavedCount.Should().Be(_expectedResult.SavedCount);
        }
    }
}
