using FluentAssertions;
using Lrs.LearnerService.Api.Client;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.LearnerService
{
    public class When_Called_With_Empty_Data : TestSetup
    {
        private List<RegisteredLearnerDetails> _registrationLearnerDetails;
        private LearnerGenderResponse _expectedResult;
        private findLearnerByULNResponse _apiResponse;

        public override void Given()
        {
            _apiResponse = null;
            _registrationLearnerDetails = new List<RegisteredLearnerDetails>();
            LearnerRecordService.GetPendingGenderLearnersAsync().Returns(_registrationLearnerDetails);

            LearnerServiceApiClient.FetchLearnerDetailsAsync(Arg.Any<RegisteredLearnerDetails>()).Returns(_apiResponse);

            _expectedResult = new LearnerGenderResponse { IsSuccess = true, TotalCount = 0, LrsCount = 0, ModifiedCount = 0, SavedCount = 0 };
            LearnerRecordService.ProcessLearnerGenderAsync(Arg.Any<List<LearnerRecordDetails>>()).Returns(_expectedResult);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            LearnerRecordService.Received(1).GetPendingGenderLearnersAsync();
            LearnerServiceApiClient.DidNotReceive().FetchLearnerDetailsAsync(Arg.Any<RegisteredLearnerDetails>());
            LearnerRecordService.DidNotReceive().ProcessLearnerGenderAsync(Arg.Any<List<LearnerRecordDetails>>());
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
