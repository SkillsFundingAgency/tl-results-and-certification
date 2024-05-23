using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.CertificateTrackingExtractionTests
{
    public class When_Service_Response_Is_Successful : TestSetup
    {
        public override void Given()
        {
            CommonService.CreateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
            Service.ProcessCertificateTrackingExtractAsync(Arg.Any<Func<DateTime>>(), Arg.Any<Func<string>>()).Returns(new FunctionResponse { IsSuccess = true, Message = "Success message." });
            CommonService.UpdateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            CommonService.Received(1).CreateFunctionLog(Arg.Any<FunctionLogDetails>());
            Service.Received(1).ProcessCertificateTrackingExtractAsync(Arg.Any<Func<DateTime>>(), Arg.Any<Func<string>>());
            CommonService.Received(1).UpdateFunctionLog(Arg.Any<FunctionLogDetails>());
        }
    }
}