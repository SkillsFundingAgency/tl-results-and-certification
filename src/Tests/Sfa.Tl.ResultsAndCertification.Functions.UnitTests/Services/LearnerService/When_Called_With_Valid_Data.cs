using FluentAssertions;
using Lrs.LearnerService.Api.Client;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.LearnerService
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private List<RegisteredLearnerDetails> _registrationLearnerDetails;
        private LearnerGenderResponse _expectedResult;
        private findLearnerByULNResponse _apiResponse;

        public override void Given()
        {
            var registrationLearnerDetails = new RegisteredLearnerDetails
            {
                ProfileId = 1,
                Uln = 9875641231,
                Firstname = "Test First 1",
                Lastname = "Test Last 1",
                DateofBirth = DateTime.UtcNow.AddYears(-40)
            };

            _registrationLearnerDetails = new List<RegisteredLearnerDetails> { registrationLearnerDetails };
            LearnerRecordService.GetPendingGenderLearnersAsync().Returns(_registrationLearnerDetails);

            _apiResponse = new findLearnerByULNResponse { FindLearnerResponse = new FindLearnerResp { ULN = registrationLearnerDetails.Uln.ToString()  } };
            LearnerServiceApiClient.FetchLearnerDetailsAsync(Arg.Any<RegisteredLearnerDetails>()).Returns(_apiResponse);

            _expectedResult = new LearnerGenderResponse { IsSuccess = true, TotalCount = 1, LrsCount = 1, ModifiedCount = 1, SavedCount = 1 };
            LearnerRecordService.ProcessLearnerGenderAsync(Arg.Any<List<LearnerRecordDetails>>()).Returns(_expectedResult);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            LearnerRecordService.Received(1).GetPendingGenderLearnersAsync();
            LearnerServiceApiClient.Received(1).FetchLearnerDetailsAsync(Arg.Any<RegisteredLearnerDetails>());
            LearnerRecordService.Received(1).ProcessLearnerGenderAsync(Arg.Any<List<LearnerRecordDetails>>());
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
