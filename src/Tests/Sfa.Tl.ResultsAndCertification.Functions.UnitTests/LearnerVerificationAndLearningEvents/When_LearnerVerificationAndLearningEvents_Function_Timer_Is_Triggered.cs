using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.LearnerVerificationAndLearningEvents
{
    public class When_LearnerVerificationAndLearningEvents_Function_Timer_Is_Triggered
    {
        private readonly IPersonalLearningRecordService _personalLearningRecordService;
        private readonly ICommonService _commonService;

        public When_LearnerVerificationAndLearningEvents_Function_Timer_Is_Triggered()
        {
            var timerSchedule = Substitute.For<TimerSchedule>();
            
            _commonService = Substitute.For<ICommonService>();
            _personalLearningRecordService = Substitute.For<IPersonalLearningRecordService>();
            
            _commonService.CreateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);

            _personalLearningRecordService.ProcessLearnerVerificationAndLearningEvents().Returns(new LearnerVerificationAndLearningEventsResponse { IsSuccess = true });

            _commonService.UpdateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);

            var learningEventsFunction = new Functions.LearnerVerificationAndLearningEvents(_commonService, _personalLearningRecordService);

            learningEventsFunction.VerifyLearnerAndFetchLearningEventsAsync(new TimerInfo(timerSchedule, new ScheduleStatus()), new ExecutionContext(), new NullLogger<Functions.LearnerVerificationAndLearningEvents>()).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            _commonService.Received(1).CreateFunctionLog(Arg.Any<FunctionLogDetails>());
            _personalLearningRecordService.Received(1).ProcessLearnerVerificationAndLearningEvents();
            _commonService.Received(1).UpdateFunctionLog(Arg.Any<FunctionLogDetails>());
        }       
    }
}
