using Microsoft.Azure.Functions.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;


namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.IndustryPlacementExtractTests
{
    public abstract class IndustryPlacementExtractFunctionTestBase : BaseTest<IndustryPlacementExtract>
    {
        // Depedencies
        protected ILogger<IndustryPlacementNotificationService> Logger;
        protected FunctionContext FunctionContext;
        protected IIndustryPlacementService IndustryPlacementService;
        protected ICommonService CommonService;

        // Actual function instance
        protected IndustryPlacementExtract IndustryPlacementExtractFunction;

        public override void Setup()
        {
            Logger = Substitute.For<ILogger<IndustryPlacementNotificationService>>();
            FunctionContext = Substitute.For<FunctionContext>();
            IndustryPlacementService = Substitute.For<IIndustryPlacementService>();
            CommonService = Substitute.For<ICommonService>();

            IndustryPlacementExtractFunction = new IndustryPlacementExtract(IndustryPlacementService, CommonService, Logger);
        }

    }
}
