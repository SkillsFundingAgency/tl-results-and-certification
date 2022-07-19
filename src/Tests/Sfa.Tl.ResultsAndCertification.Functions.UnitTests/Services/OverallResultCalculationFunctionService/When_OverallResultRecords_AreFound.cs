using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.OverallResultCalculationFunctionService
{
    public class When_OverallResultRecords_AreFound : TestSetup
    {
        private List<OverallResultResponse> _expectedResult;

        public override void Given()
        {
            _expectedResult = new List<OverallResultResponse> 
            { 
                new OverallResultResponse { IsSuccess = true, TotalRecords = 5, NewRecords = 5, UpdatedRecords = 0, SavedRecords = 5, UnChangedRecords = 0  },
                new OverallResultResponse { IsSuccess = true, TotalRecords = 3, NewRecords = 1, UpdatedRecords = 2, SavedRecords = 3, UnChangedRecords = 0  },
            };

            OverallResultCalculationService.CalculateOverallResultsAsync(Arg.Any<DateTime>()).Returns(_expectedResult);
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
            ActualResult.Should().HaveCount(2);

            foreach (var (result, index) in ActualResult.Select((value, i) => (value, i)))
            {
                result.IsSuccess.Should().BeTrue();
                result.TotalRecords.Should().Be(_expectedResult[index].TotalRecords);
                result.NewRecords.Should().Be(_expectedResult[index].NewRecords);
                result.UpdatedRecords.Should().Be(_expectedResult[index].UpdatedRecords);
                result.UnChangedRecords.Should().Be(_expectedResult[index].UnChangedRecords);
                result.SavedRecords.Should().Be(_expectedResult[index].SavedRecords);
            }
        }
    }
}
