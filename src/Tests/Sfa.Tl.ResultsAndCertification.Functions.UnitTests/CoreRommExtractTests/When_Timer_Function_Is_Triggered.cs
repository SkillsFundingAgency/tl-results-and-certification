using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.CoreRommExtractTests
{
    public class When_Timer_Function_Is_Triggered : TestSetup
    {
        public override void Given()
        {
            CommonService.CreateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
            CoreRommExtractService.ProcessCoreRommExtractAsync(AssessmentSeriesYearsToProcess).Returns(new FunctionResponse { IsSuccess = true, Message = "Success message." });
            CommonService.UpdateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            CommonService.Received(1).CreateFunctionLog(Arg.Any<FunctionLogDetails>());
            CoreRommExtractService.Received(1).ProcessCoreRommExtractAsync(AssessmentSeriesYearsToProcess);
            CommonService.Received(1).UpdateFunctionLog(Arg.Any<FunctionLogDetails>());
        }
    }
}