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
        protected TimerSchedule TimerSchedule = Substitute.For<TimerSchedule>();
        protected IAnalystCoreResultExtractionService AnalystCoreResultExtractionService = Substitute.For<IAnalystCoreResultExtractionService>();
        protected ICommonService CommonService = Substitute.For<ICommonService>();

        // Actual function instance
        protected AnalystCoreResultExtraction AnalystCoreResultExtractionFunction;
    }
}
