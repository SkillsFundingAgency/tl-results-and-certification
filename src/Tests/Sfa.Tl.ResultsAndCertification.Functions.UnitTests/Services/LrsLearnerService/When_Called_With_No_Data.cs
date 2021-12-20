using FluentAssertions;
using Lrs.LearnerService.Api.Client;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.LrsLearnerService
{
    public class When_Called_With_No_Data : TestSetup
    {
        private List<RegisteredLearnerDetails> _registrationLearnerDetails;
        private LrsLearnerGenderResponse _expectedResult;
        private findLearnerByULNResponse _apiResponse;

        public override void Given()
        {
            _apiResponse = null;
            _registrationLearnerDetails = null;
            LrsService.GetPendingGenderLearnersAsync().Returns(_registrationLearnerDetails);

            LrsLearnerServiceApiClient.FetchLearnerDetailsAsync(Arg.Any<RegisteredLearnerDetails>()).Returns(_apiResponse);

            _expectedResult = new LrsLearnerGenderResponse { IsSuccess = true, TotalCount = 0, LrsCount = 0, ModifiedCount = 0, SavedCount = 0 };
            LrsService.ProcessLearnerGenderAsync(Arg.Any<List<LrsLearnerRecordDetails>>()).Returns(_expectedResult);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            LrsService.Received(1).GetPendingGenderLearnersAsync();
            LrsLearnerServiceApiClient.DidNotReceive().FetchLearnerDetailsAsync(Arg.Any<RegisteredLearnerDetails>());
            LrsService.DidNotReceive().ProcessLearnerGenderAsync(Arg.Any<List<LrsLearnerRecordDetails>>());
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
