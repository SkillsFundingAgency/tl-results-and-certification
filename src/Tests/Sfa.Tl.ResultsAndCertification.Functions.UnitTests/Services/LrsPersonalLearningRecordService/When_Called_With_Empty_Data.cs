﻿using FluentAssertions;
using Lrs.PersonalLearningRecordService.Api.Client;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.LrsPersonalLearningRecordService
{
    public class When_Called_With_Empty_Data : TestSetup
    {
        private List<RegisteredLearnerDetails> _registrationLearnerDetails;
        private LrsLearnerVerificationAndLearningEventsResponse _expectedResult;
        private GetLearnerLearningEventsResponse _apiResponse;

        public override void Given()
        {
            _apiResponse = null;
            _registrationLearnerDetails = new List<RegisteredLearnerDetails>();
            LrsService.GetPendingVerificationAndLearningEventsLearnersAsync().Returns(_registrationLearnerDetails);

            LrsPersonalLearningRecordApiClient.GetLearnerEventsAsync(Arg.Any<RegisteredLearnerDetails>()).Returns(_apiResponse);

            _expectedResult = new LrsLearnerVerificationAndLearningEventsResponse { IsSuccess = true, TotalCount = 0, LrsCount = 0, ModifiedCount = 0, SavedCount = 0 };
            LrsService.ProcessLearnerRecordsAsync(Arg.Any<List<LrsLearnerRecordDetails>>()).Returns(_expectedResult);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            LrsService.Received(1).GetPendingVerificationAndLearningEventsLearnersAsync();
            LrsPersonalLearningRecordApiClient.DidNotReceive().GetLearnerEventsAsync(Arg.Any<RegisteredLearnerDetails>());
            LrsService.DidNotReceive().ProcessLearnerRecordsAsync(Arg.Any<List<LrsLearnerRecordDetails>>());
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
