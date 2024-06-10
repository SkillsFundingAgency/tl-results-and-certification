using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.CertificateTrackingExtractionTests
{
    public class When_Service_Throws_Exception : TestSetup
    {
        public override void Given()
        {
            CommonService.CreateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
            Service.ProcessCertificateTrackingExtractAsync(Arg.Any<Func<DateTime>>(), Arg.Any<Func<string>>()).Returns(x => Task.FromException(new Exception()));
            CommonService.UpdateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            CommonService.Received(2).CreateFunctionLog(Arg.Any<FunctionLogDetails>());
            Service.Received(1).ProcessCertificateTrackingExtractAsync(Arg.Any<Func<DateTime>>(), Arg.Any<Func<string>>());
            CommonService.DidNotReceive().UpdateFunctionLog(Arg.Any<FunctionLogDetails>());
            CommonService.Received(1).SendFunctionJobFailedNotification(Arg.Any<string>(), Arg.Any<string>());
        }
    }
}