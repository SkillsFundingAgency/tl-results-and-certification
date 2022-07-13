using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.OverallResultCalculationFunctionService
{
    public class When_OverallResultRecords_IsNull : TestSetup
    {
        private List<OverallResultResponse> _mockData;
        private List<OverallResultResponse> _expectedResult;
        private DateTime _runDate;

        public override void Given()
        {
            _mockData = null;
            _expectedResult = new List<OverallResultResponse> { new OverallResultResponse { IsSuccess = true, NewRecords = 0, TotalRecords = 0, SavedRecords = 0, UnChangedRecords = 0, UpdatedRecords = 0 } };

            _runDate = DateTime.UtcNow.Date;
            OverallResultCalculationService.CalculateOverallResultsAsync(_runDate).Returns(_mockData);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            OverallResultCalculationService.Received(1).CalculateOverallResultsAsync(Arg.Any<DateTime>());
        }

        [Fact]
        public void Then_Expected_Response_Returned()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.Should().HaveCount(1);
            ActualResult[0].IsSuccess.Should().BeTrue();
            ActualResult[0].TotalRecords.Should().Be(_expectedResult[0].TotalRecords);
            ActualResult[0].NewRecords.Should().Be(_expectedResult[0].TotalRecords);
            ActualResult[0].UpdatedRecords.Should().Be(_expectedResult[0].TotalRecords);
            ActualResult[0].UnChangedRecords.Should().Be(_expectedResult[0].TotalRecords);
            ActualResult[0].SavedRecords.Should().Be(_expectedResult[0].TotalRecords);
        }
    }
}
