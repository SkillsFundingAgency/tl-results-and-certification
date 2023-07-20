using Microsoft.Azure.WebJobs.Extensions.Timers;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;


namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.AnalystCoreResultExtractionService
{
    public abstract class AnalystCoreResultExtractFunctionTestBase : BaseTest<AnalystCoreResultExtraction>
    {
        // Depedencies
        protected TimerSchedule TimerSchedule;
        protected IAnalystCoreResultExtractionService AnalystCoreResultExtractionService;
        protected ICommonService CommonService;

        // Actual function instance
        protected AnalystCoreResultExtraction AnalystCoreResultExtractionFunction;

        public override void Setup()
        {
            TimerSchedule = Substitute.For<TimerSchedule>();
            AnalystCoreResultExtractionService = Substitute.For<IAnalystCoreResultExtractionService>();
            CommonService = Substitute.For<ICommonService>();

            AnalystCoreResultExtractionFunction = new AnalystCoreResultExtraction(AnalystCoreResultExtractionService, CommonService);
        }

    }
}
