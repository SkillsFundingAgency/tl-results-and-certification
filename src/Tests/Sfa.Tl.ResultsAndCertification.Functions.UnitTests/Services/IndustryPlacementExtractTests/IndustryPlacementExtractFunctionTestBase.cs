using Microsoft.Azure.WebJobs.Extensions.Timers;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;


namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.IndustryPlacementExtractTests
{
    public abstract class IndustryPlacementExtractFunctionTestBase : BaseTest<IndustryPlacementExtract>
    {
        // Depedencies
        protected TimerSchedule TimerSchedule;
        protected IIndustryPlacementService IndustryPlacementService;
        protected ICommonService CommonService;

        // Actual function instance
        protected IndustryPlacementExtract IndustryPlacementExtractFunction;

        public override void Setup()
        {
            TimerSchedule = Substitute.For<TimerSchedule>();
            IndustryPlacementService = Substitute.For<IIndustryPlacementService>();
            CommonService = Substitute.For<ICommonService>();

            IndustryPlacementExtractFunction = new IndustryPlacementExtract(IndustryPlacementService, CommonService);
        }
       
    }
}
