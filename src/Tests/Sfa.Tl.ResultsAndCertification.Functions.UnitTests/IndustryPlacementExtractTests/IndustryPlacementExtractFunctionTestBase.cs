using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;


namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.IndustryPlacementExtractTests
{
    public abstract class IndustryPlacementExtractFunctionTestBase : BaseTest<IndustryPlacementExtract>
    {
        // Depedencies
        protected IIndustryPlacementService IndustryPlacementService;
        protected ICommonService CommonService;

        // Actual function instance
        protected IndustryPlacementExtract IndustryPlacementExtractFunction;

        public override void Setup()
        {
            IndustryPlacementService = Substitute.For<IIndustryPlacementService>();
            CommonService = Substitute.For<ICommonService>();

            IndustryPlacementExtractFunction = new IndustryPlacementExtract(IndustryPlacementService, CommonService);
        }

    }
}
