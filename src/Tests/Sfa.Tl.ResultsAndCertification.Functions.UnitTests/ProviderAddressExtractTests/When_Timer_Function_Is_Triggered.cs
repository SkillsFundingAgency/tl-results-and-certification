using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.ProviderAddressExtractTests

{
    public class When_Timer_Function_Is_Triggered : TestSetup
    {
        public override void Given()
        {
            CommonService.CreateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
            ProviderAddressExtractionService.ProcessProviderAddressExtractionAsync(AcademicYearsToProcess).Returns(new FunctionResponse { IsSuccess = true, Message = "Success message." });
            CommonService.UpdateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            CommonService.Received(1).CreateFunctionLog(Arg.Any<FunctionLogDetails>());
            ProviderAddressExtractionService.Received(1).ProcessProviderAddressExtractionAsync(AcademicYearsToProcess);
            CommonService.Received(1).UpdateFunctionLog(Arg.Any<FunctionLogDetails>());
        }
    }
}