using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.AnalystCoreResultExtractionService
{
    public class When_Service_Exception : TestSetup
    {
        public override void Given()
        {
            var todayDate = "20/07/2023".ParseStringToDateTimeWithFormat();
            CommonService.CurrentDate.Returns(todayDate);
                
            CommonService.CreateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
            CommonService.IsAnalystCoreResultExtractionTriggerValid().Returns(true);            
            AnalystCoreResultExtractionService.ProcessAnalystCoreResultExtractsAsync().Returns(x => Task.FromException(new Exception()));
            CommonService.UpdateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            CommonService.Received(2).CreateFunctionLog(Arg.Any<FunctionLogDetails>());
            AnalystCoreResultExtractionService.Received(1).ProcessAnalystCoreResultExtractsAsync();
            CommonService.DidNotReceive().UpdateFunctionLog(Arg.Any<FunctionLogDetails>());
            CommonService.Received(1).SendFunctionJobFailedNotification(Arg.Any<string>(), Arg.Any<string>());
        }
    }
}