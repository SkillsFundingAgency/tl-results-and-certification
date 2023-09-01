using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.CoreRommExtractTests
{
    public class When_Service_Exception : TestSetup
    {
        public override void Given()
        {
            CommonService.CreateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
            CoreRommExtractService.ProcessCoreRommExtractAsync(AssessmentSeriesYearsToProcess).Returns(x => Task.FromException(new Exception()));
            CommonService.UpdateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            CommonService.Received(2).CreateFunctionLog(Arg.Any<FunctionLogDetails>());
            CoreRommExtractService.Received(1).ProcessCoreRommExtractAsync(AssessmentSeriesYearsToProcess);
            CommonService.DidNotReceive().UpdateFunctionLog(Arg.Any<FunctionLogDetails>());
            CommonService.Received(1).SendFunctionJobFailedNotification(Arg.Any<string>(), Arg.Any<string>());
        }
    }
}